using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]

public class ShapeData : ScriptableObject 
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
            column = new bool[rowSize];

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
    public Row[] shapeBoard;

    //completely clears board of shapes
    public void Clear()
    {
        for(int i = 0; i < rowCount; i++)
        {
            shapeBoard[i].ClearRow();
        }
    }

    //creates a new shape board according to number of columns
    public void CreateShapeBoard()
    {
        shapeBoard = new Row[rowCount];

        for(int i = 0; i < rowCount; i++)
        {
            shapeBoard[i] = new Row(columnCount);
        }
    }
}
