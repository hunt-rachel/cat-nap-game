using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    //public variables
    public int columnCount = 0;
    public int rowCount = 0;

    public float gapSize = 0.1f;
    public float squareScale = 0.5f;
    public float squareOffset = 0.0f;

    public GameObject gridSquare;
    
    public Vector2 startPos = new Vector2(0.0f, 0.0f);

    //private variables
    private Vector2 offset = new Vector2(0.0f, 0.0f);

    private List<GameObject> gridSquaresList = new List<GameObject>();
    
    void Start()
    {
        CreateGrid();
    }

    private void OnEnable()
    {
        GameEvents.CheckIfShapePlacable += CheckIfShapePlacable;
    }

    private void OnDisable()
    {
        GameEvents.CheckIfShapePlacable -= CheckIfShapePlacable;
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        PositionGridSquares();
    }

    private void SpawnGridSquares()
    {
        int squareIndex = 0;

        for (int row = 0; row < rowCount; ++row)
        {
            for(int col = 0; col < columnCount; ++col)
            {
                //instantiate grid squares as children of grid canvas object
                gridSquaresList.Add(Instantiate(gridSquare) as GameObject);
                gridSquaresList[gridSquaresList.Count - 1].transform.SetParent(this.transform);

                //set square features
                gridSquaresList[gridSquaresList.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);

                //gridSquaresList[gridSquaresList.Count - 1].GetComponent<GridSquare>().SetImage(squareIndex % 2 == 0);
                squareIndex++;
            }
        }
    }

    //sets position for spawned squares to create grid
    private void PositionGridSquares()
    {
        //variable definitions
        int colNum = 0;
        int rowNum = 0;

        Vector2 gapNumber = new Vector2(0.0f, 0.0f);

        bool rowMoved = false;

        var squareRect = gridSquaresList[0].GetComponent<RectTransform>();

        //set x and y offset for each square in grid
        offset.x = squareRect.rect.width * squareRect.transform.localScale.x + squareOffset;
        offset.y = squareRect.rect.height * squareRect.transform.localScale.y + squareOffset;

        foreach(GameObject square in gridSquaresList)
        {
            if(colNum + 1 > columnCount)
            {
                //if reached right edge of grid, reset column count to left and increment row
                gapNumber.x = 0;
                colNum = 0;
                rowNum++;
                
                //resets for future check
                rowMoved = false;
            }

            float xPosOffset = offset.x * colNum + (gapNumber.x * gapSize);
            float yPosOffset = offset.y * rowNum + (gapNumber.y * gapSize);

            if (colNum > 0)
            {
                gapNumber.x++;
                xPosOffset += gapSize;
            }

            if (rowNum > 0 && rowMoved == false)
            {
                rowMoved = true;
                gapNumber.y++;
                yPosOffset += gapSize;
            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPos.x + xPosOffset, startPos.y - yPosOffset);
            square.GetComponent<RectTransform>().localPosition = new Vector3(startPos.x + xPosOffset, startPos.y - yPosOffset, 0.0f);

            var squareImg = square.transform.Find("Default Image");
            
            /*for debugging
            if(squareImg != null)
            {
                Debug.Log("found square image in column " + colNum + " and row " + rowNum);
            }*/

            //set colours to define edges of grid
            //if leftmost / rightmost / topmost / bottommost
            if(colNum == 0 || colNum == columnCount - 1 || rowNum == 0 || rowNum == rowCount - 1)
            {
                squareImg.GetComponent<Image>().color = Color.red;
            }

            else
            {
                squareImg.GetComponent<Image>().color = Color.blue;
            }


            colNum++;
        }
    }

    private void CheckIfShapePlacable()
    {
        foreach(var square in gridSquaresList)
        {
            var gridSquare = square.GetComponent<GridSquare>();

            if(gridSquare.PlacableHere() == true)
            {
                gridSquare.ActivateSquare();
            }
        }
    }
}
