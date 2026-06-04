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

    public Vector3Int startPos;

    //TODO: replace with random background + select colour from colour pallete once artwork underway
    public Sprite squareSprite;
    public Color shapeColour;

    private void Awake()
    {
        board = GameObject.Find("Game Board Grid").GetComponentInChildren<Board>();
        //if (board) { Debug.Log("found the board"); }
        gm = GameObject.Find("Game Board Grid").GetComponent<GameManager>();
        //if (gm) { Debug.Log("found the game manager script"); }

        sm = GameObject.Find("Shapes Manager").GetComponent<ShapeManager>();
        //if (sm) { Debug.Log("found shape manager"); }
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
}
