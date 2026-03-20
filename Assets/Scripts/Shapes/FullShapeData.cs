using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]

public class FullShapeData : ScriptableObject 
{
    [System.Serializable]
    public class Row
    {
        public bool[] column;
        private int rowSize = 0;

        //empty constructor
        public Row() { }

        //constructor with size parsed
        public Row(int size)
        {
            CreateRow(size);
        }

        //creates new row with provided size
        public void CreateRow(int size)
        {
            rowSize = size;
            column; new bool[rowSize];

            //clears data to be ready for next row created
            ClearRow();
        }

        //clears row based on provided size
        public void ClearRow()
        {
            for(int i = 0; i < rowSize; i++)
            {
                column[i] = false;
            }
        }
    }

    public int columnCount = 0;
    public int rowCount = 0;
    public Row[] gameBoard;

    //completely clears board of shapes
    public void Clear()
    {
        for(int i = 0; i < rowCount; i++)
        {
            gameBoard[i].ClearRow();
        }
    }

    //creates a new game board according to number of columns
    public void CreateGameBoard()
    {
        gameBoard = new Row[rowCount];

        for(int i = 0; i < rowCount; i++)
        {
            gameBoard[i] = new Row(columnCount);
        }
    }
}
