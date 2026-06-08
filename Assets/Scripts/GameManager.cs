using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    //default board size, can be changed in inspector. includes edge rows and cols
    public int width;
    public int height;

    public Tilemap tilemap;

    private Board board;

    [SerializeField] public Cell[,] state;

    public bool gameOver; //bool for when no more actions can be taken in game
    public bool canPlaceShape = true; //when false, game over

    public ShapeManager sm;

    //TODO: add game ui reference here when needed

    private void Awake()
    {
        board = GetComponentInChildren<Board>();
    }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        //player can press R on keyboard to restart
        //TODO: make mobile friendly input when desktop version complete
        if(Input.GetKeyDown(KeyCode.R))
        {
            NewGame();
        }

        else if(!gameOver)
        {
            //TODO: add mouse interaciton actions here. e.g. what happens when player clicks
        }
    }

    //creates data for when player starts a new game
    private void NewGame()
    {
        Debug.Log("starting a new game!");
        gameOver = false;

        state = new Cell[width, height];

        GenerateCells();

        board.DrawBoard(state);

        //clear current shapes to place on board if any
        if(sm.currPlayableShapes.Count > 0)
        {
            sm.currPlayableShapes.Clear();
        } 
        
        sm.SetShapes();
        sm.SetEmptySpace();
    }

    private void GenerateCells()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Cell cell = new Cell(); //create cell reference
                cell.pos = new Vector3Int(x, y, 0); //set cell position on game grid
                //Debug.Log("cell pos at x: " + x + ", y: " + y + " is " + cell.pos);
                
                //set edges on instantiation
                if(x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    cell.type = Cell.Type.Edge;
                    cell.isEdge = true;
                    //Debug.Log("cell should be edge. cell type: " + cell.type);
                }

                else
                {
                    cell.type = Cell.Type.Empty; //instantaite cell as empty, so can be filled when shapes are placed
                    //Debug.Log("not an edge. cell type: " + cell.type);
                }

                state[x, y] = cell;
                //Debug.Log("generated cell at pos x: " + x + ", y: " + y);
            }
        }
    }
    
    //check whether cell is valid & within bounds of the board
    private bool IsValidCell(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
    
    //method to return cell type at certain position
    public Cell GetCell(int x, int y)
    {
        if(IsValidCell(x,y))
        {
            return state[x, y];
        }

        else
        {
            Debug.Log("not a valid cell");
            return new Cell();
        }
    }

    //checks if current empty space has been made
    public bool CheckIfEmptySpaceMade()
    {
        EmptySpace es = sm.currEmptySpace.GetComponent<EmptySpace>();
        //if (es) { Debug.Log("found current empty space script"); }

        Debug.Log("es X bound: " + es.xBound + ", es Y above bound: " + es.yAboveBound + ", es below bound: " + es.yBelowBound);
        
        //iterate over board
        for(int x = 0; x < width; x++)
        {
            //checks potential border position would be within bounds of board
            if (x + es.xBound > width) 
            {
                Debug.Log("BREAKING: x would now be out of bounds when checking");
                break; 
            }

            for (int y = 0; y < height; y++)
            {
                Debug.Log("checking cell for empty at position x: " + x + ", y: " + y);

                //checks potential border position would be within bounds of board
                if (y - es.yBelowBound < 0 || y + es.yAboveBound > height) 
                {
                    Debug.Log("CONTINUING: y would now be out of bounds when checking");
                    continue; 
                }

                Cell currCell = state[x, y];
                
                //skip checking edge or filled cells, as they cannot be empty space
                if(currCell.isEdge || currCell.filled) 
                {
                    Debug.Log("CONTINUING: current cell is either edge or filled");
                    continue; 
                }

                //set initial start point for checking border, x - 1 as always start checking to left of empty space
                Vector2Int checkStartPos = new Vector2Int(x - 1, y);
                Debug.Log("checking border start path at x: " + checkStartPos.x + ", y: " + checkStartPos.y);

                //temp int to see how many border cells have been checked
                int bordersChecked = 0;

                //loop over border path of empty space
                foreach(Vector2Int currBorderPoint in es.borderPath)
                {
                    int tempX = checkStartPos.x + currBorderPoint.x;
                    int tempY = checkStartPos.y + currBorderPoint.y;

                    //if current point in border check path is not filled in some way, empty space cannot have been filled
                    if (state[tempX, tempY].type == Cell.Type.Empty)
                    {
                        Debug.Log("current cell type: " + state[tempX, tempY].type);
                        Debug.Log("there is a break in the border at position x: " + tempX + ", y: " + tempY);
                        break;
                    }

                    Debug.Log("cell at position x: " + tempX + ", y: " + tempY + " is edge? " + state[tempX, tempY].isEdge + ", is filled? " + state[tempX, tempY].filled);
                    bordersChecked++;
                }

                //if amount of borders checked == border path count without breaking, means entire border is filled, and empty space has been made
                if(bordersChecked == es.borderPath.Count)
                {
                    Debug.Log("all borders checked, the empty space has successfully been made!");
                    return true;
                }
            }
        }

        //entire border was checked without any breaks, meaining empty space has been created
        return false;
    }

}
