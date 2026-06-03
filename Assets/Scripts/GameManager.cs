using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    //default board size, can be changed in inspector. includes edge rows and cols
    public int width = 7;
    public int height = 7;

    public Tilemap tilemap;

    private Board board;

    private Cell[,] state;


    public bool gameOver; //bool for when no more actions can be taken in game
    public bool canPlaceShape = true; //when false, game over

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
    }

    private void GenerateCells()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Cell cell = new Cell(); //create cell reference
                cell.pos = new Vector3Int(x, y, 0); //set cell position on game grid
                cell.type = Cell.Type.Empty; //instantaite cell as empty, so can set type according to state

                state[x, y] = cell;
                //Debug.Log("generated cell at pos x: " + x + ", y: " + y);
            }
        }
    }

    /* POTENTIALLY RELEVANT METHODS FOR WHEN IT COMES TO BOARD CLEARING
    
    //check whether cell is valid & within bounds of the board
    private bool IsValidCell(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
    
    //method to return cell type at certain position
    private Cell GetCell(int x, int y)
    {
        if(IsValidCell(x,y))
        {
            return state[x,y]
        }

        else
        {
            return new Cell();
        }
    }*/
}
