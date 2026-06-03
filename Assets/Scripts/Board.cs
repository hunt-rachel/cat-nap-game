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
                tilemap.SetTile(cell.pos, GetTileType(cell, x, y, width, height));
            }
        }
    }

    //return what tile type to display when game board is drawn
    private Tile GetTileType(Cell cell, int x, int y, int w, int h)
    {
        //filled cell - shape has been placed over tile
        if(cell.filled)
        {
            return tileFilled;
        }

        //edge cell - always filled, but can't place shapes over it
        else if(x == 0 || x == w - 1 || y == 0 || y == h - 1)
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
