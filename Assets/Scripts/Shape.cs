using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Shape : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    private Board board;
    private GameManager gm;
    private ShapeManager sm;
    private EmptySpace es;

    public Vector3Int startPos;
    [Space]
    [Space]

    //TODO: replace with random background + select colour from colour pallete once artwork underway
    public Sprite squareSprite;
    public Color shapeColour;
    [Space]
    [Space]

    public bool canBePlaced = true;
    [Space]
    [Space]

    public int rotationCount; //how many variants of shape paths need to be stored

    public List<Vector2Int> shapePath0Deg;
    public List<Vector2Int> shapePath90Deg;
    public List<Vector2Int> shapePath180Deg;
    public List<Vector2Int> shapePath270Deg;

    public List<List<Vector2Int>> shapePathsMaster; //coordinate +- ints here when checking


    private void Awake()
    {
        board = GameObject.Find("Game Board Grid").GetComponentInChildren<Board>();
        //if (board) { Debug.Log("found the board"); }
        gm = GameObject.Find("Game Board Grid").GetComponent<GameManager>();
        //if (gm) { Debug.Log("found the game manager script"); }
        sm = GameObject.Find("Shapes Manager").GetComponent<ShapeManager>();
        //if (sm) { Debug.Log("found shape manager"); }
    }

    private void Start()
    {
        es = sm.currEmptySpace.GetComponent<EmptySpace>();
        //if (es) { Debug.Log("found empty shape script reference"); }

        shapePathsMaster = new List<List<Vector2Int>>();

        shapePathsMaster.Add(shapePath0Deg);
        shapePathsMaster.Add(shapePath90Deg);
        shapePathsMaster.Add(shapePath180Deg);
        shapePathsMaster.Add(shapePath270Deg);
    }

    private void Update()
    {
        //TODO: check whether current shapes provided can be placed on board, if not, game over
        canBePlaced = CheckIfShapeCanBePlaced();

        gm.canPlaceAnyShape = Convert.ToBoolean(Math.Min(Convert.ToInt32(gm.canPlaceAnyShape), Convert.ToInt32(canBePlaced)));
    }

    public void SetShapeFeatures()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            SpriteRenderer childSpre = this.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
            //if (childSpre) { Debug.Log("found sprite renderer of child " + i); }

            childSpre.sprite = squareSprite;
            childSpre.color = shapeColour;
        }

        this.transform.position = startPos;
    }

    void OnMouseDown()
    {
        Vector3 pos = this.transform.position;
        
        screenPoint = Camera.main.WorldToScreenPoint(pos);
        offset = pos - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 currPoint = new Vector3(mousePos.x, mousePos.y, screenPoint.z);

        Vector3 currPos = Camera.main.ScreenToWorldPoint(currPoint) + offset;

        //maybe remove depending on if it works well
        if(Input.GetMouseButtonUp(1))
        {
            this.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);
        }

        this.transform.position = currPos;
    }

    void OnMouseUp()
    {
        PlaceShape();
        
        //checks if current empty space can be made at any point on board, if not, game over
        bool isGameOver = CheckIfSpaceCanBeMade();

        if(isGameOver)
        {
            gm.gameOver = true;
        }

        else
        {
            /*//change to vector2int pos and (-1, -1) will represent false, num will represent coordinate to start border handling otherwise
            Vector2Int emptyStartPos = gm.CheckIfEmptySpaceMade();

            if (emptyStartPos != new Vector2Int(-1, -1))
            {
                Vector2Int borderStartPos = new Vector2Int(emptyStartPos.x - 1, emptyStartPos.y);
                
                gm.HandleBorderScoring(es, borderStartPos);

                //call to set new empty space
                sm.SetEmptySpace();
            }*/

            List<Vector2Int> bordersToCheckList = gm.CheckIfEmptySpaceMade();

            if(bordersToCheckList.Count != 0)
            {
                gm.HandleBorderScoring(es, bordersToCheckList);

                //call to set new empty space
                sm.SetEmptySpace();
            }
        }
    }

    private void PlaceShape()
    {
        Vector3 shapePos = this.transform.position;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(shapePos);

        List<Cell> cellsToFill = new List<Cell>();

        bool canPlaceShape = CheckShapePlacement(cellsToFill);

        if(canPlaceShape)
        {
            //help with snapping to grid
            int snapX = Mathf.RoundToInt(shapePos.x);
            int snapY = Mathf.RoundToInt(shapePos.y);

            this.transform.position = new Vector3(snapX, snapY, screenPoint.z);

            for (int i = 0; i < cellsToFill.Count; i++)
            {
                int stateX = cellsToFill[i].pos.x;
                int stateY = cellsToFill[i].pos.y;

                gm.state[stateX, stateY].filled = true;
                gm.state[stateX, stateY].type = Cell.Type.Filled;
            }

            //increase score for every cell on board that becomes filled
            gm.HandlePlacementScoring(cellsToFill.Count);
            
            board.DrawBoard(gm.state);

            //remove from current shapes list
            sm.currPlayableShapes.Remove(this.gameObject);

            //get rid of block once placed
            Destroy(this.gameObject);
        }

        else
        {
            this.transform.position = startPos;
        }
    }

    private bool CheckShapePlacement(List<Cell> l)
    {
        //check validity of each child (individual square) of shape
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform currChild = this.transform.GetChild(i);
            int childXPos = Mathf.RoundToInt(currChild.position.x) - 1;
            int childYPos = Mathf.RoundToInt(currChild.position.y) - 1;

            //Debug.Log((childXPos) + " and " + (childYPos));

            //if picked up but placed outside of grid, cannot place shape
            if (childXPos > gm.width || childXPos < 0 || childYPos > gm.height || childYPos < 0)
            {
                return false;
            }

            Cell currCell = gm.state[childXPos, childYPos];

            //player tried to place shape in invalid space
            if (currCell.type != Cell.Type.Empty)
            {
                return false;
            }

            else
            {
                l.Add(currCell);
            }
        }

        return true;
    }

    private bool CheckIfShapeCanBePlaced()
    {
        for (int x = 0; x < gm.width; x++)
        {
            for(int y = 0; y < gm.height; y++)
            {
                List<int> pathsFromCurrPoint = new List<int>();
                pathsFromCurrPoint = CheckShapeRotationPaths(x, y);

                foreach(int i in pathsFromCurrPoint)
                {
                    
                    //if there exists a path the same length as square count in shape (after having path checked), shape can be placed
                    if(i == this.gameObject.transform.childCount)
                    {
                        return true;
                    }
                }
            }
        }
        
        //checked all paths, cannot be placed
        return false;
    }

    private List<int> CheckShapeRotationPaths(int x, int y)
    {
        List<int> returnList = new List<int>();
        
        for(int i = 0; i < rotationCount; i++)
        {
            int returnInt = 0;

            foreach (Vector2Int currPoint in shapePathsMaster[i])
            {
                int tempX = x + currPoint.x;
                int tempY = y + currPoint.y;

                //if current space not an empty space available for shape placement, shape cannot be placed starting at current point
                if (gm.state[tempX, tempY].type != Cell.Type.Empty)
                {
                    break;
                }

                returnInt++;
            }

            returnList.Add(returnInt);
        }

        return returnList;
    }
    
    private bool CheckIfSpaceCanBeMade()
    {
        for (int x = 0; x < gm.width; x++)
        {
            for (int y = 0; y < gm.height; y++)
            {
                int emptiesChecked = gm.CountSpacePath(es, x, y);

                if (emptiesChecked == es.spacePath.Count)
                {
                    return false;
                }
            }
        }

        //Debug.Log("empty space cannot be made from current position");
        //if empty space cannot be made anywhere on board, game over is true
        return true;
    }
}
