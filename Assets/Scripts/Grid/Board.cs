using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }

    //tile types
    public Tile tileEdge;
    public Tile tileEmpty;
    public Tile tileOccupied;
    public Tile tileHovering_CanPlace;
    public Tile tileHovering_CannotPlace;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void DrawBoard(Cell[,] state)
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);

        for(int col = 0; col < width; col++)
        {
            for(int row = 0; row < height; row++)
            {
                Cell cell = state[col, row];
                tilemap.SetTile(cell.pos, GetTile(cell));
            }
        }
    }

    //when drawing board, edge consists of top, left, right, and bottom most sides of board
    private Tile GetTile(Cell cell)
    {
        //for board initialisation, will add other cell types when have programmed functionality
        if(cell.type == Cell.Type.Edge)
        {
            return tileEdge;
        }
        
        else
        {
            return tileEmpty;
        }
    }
}
