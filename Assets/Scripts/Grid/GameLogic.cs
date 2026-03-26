using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public int width = 9;
    public int height = 9;

    private Board board;
    private Cell[,] state;

    public bool gameOver; //if player can no longer place shapes on board
    
    private void Awake()
    {
        board = GetComponentInChildren<Board>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        StartNewGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartNewGame()
    {
        //add relevant ui resets here
        gameOver = false;

        state = new Cell[width, height];

        //generate new game board

        //set camera for game based on board size
        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10f);

        GenerateCells();
        
        board.DrawBoard(state);
    }

    private void GenerateCells()
    {
        for(int col = 0; col < width; col++)
        {
            for(int row = 0; row < height; row++)
            {
                Cell cell = new Cell();
                cell.pos = new Vector3Int(col, row, 0);

                //if setting edge type doesn't work here, do in board class instead
                if (col == 0 || col == width - 1 || row == 0 || row == height - 1)
                {
                    cell.type = Cell.Type.Edge;
                }

                else
                {
                    cell.type = Cell.Type.Empty;
                }

                state[col, row] = cell;
            }
        }
    }
}
