using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }

    //tile types
    public Tile tileEmpty;
    public Tile tileFilled;
    public Tile tileEdge;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void DrawBoard(Cell[,] state)
    {
        //get width and height of board
        int width = state.GetLength(0);
        int height = state.GetLength(1);

        //draw the grid with correct tile states
        //TODO: set edge tiles correctly 
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];
                tilemap.SetTile(cell.pos, GetTileType(cell));
            }
        }
    }

    //return what tile type to display when game board is drawn
    private Tile GetTileType(Cell cell)
    {
        //filled cell - shape has been placed over tile
        if(cell.filled)
        {
            return tileFilled;
        }

        //edge cell - always filled, but can't place shapes over it
        else if(cell.isEdge)
        {
            return tileEdge;
        }

        //empty cell - no tile placed yet
        else
        {
            return tileEmpty;
        }
    }
}
