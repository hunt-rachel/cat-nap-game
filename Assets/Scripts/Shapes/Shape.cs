using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

//extra inheritances to help handle moving game shapes
public class Shape : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    //public vars
    public GameObject squareShapeImg;

    public Vector3 selectedShapeScale;

    //amount shape is above mouse for visibility
    public Vector2 offset = new Vector2(0f, 300f);

    [HideInInspector]
    public ShapeData currShapeData;

    //private vars
    private List<GameObject> currShape = new List<GameObject>();

    private Vector3 startScale;

    //private RectTransform rt;

    //private bool draggable = true;

    private Canvas canvas;

    public void Awake()
    {
        startScale = this.transform.localScale;
    }

    //counts number of active squares in current shape
    private int GetSquareCount(ShapeData sd)
    {
        int count = 0;

        foreach(var row in sd.shapeBoard)
        {
            foreach(var active in row.column)
            {
                if (active) { count++; }
            }
        }

        return count;
    }

    public void RequestNewShape(ShapeData sd)
    {
        CreateShape(sd);
    }
    
    //creates game shape
    public void CreateShape(ShapeData sd)
    {
        currShapeData = sd;
        int squareCount = GetSquareCount(sd);

        //adds square images to list
        while(currShape.Count <= squareCount)
        {
            currShape.Add(Instantiate(squareShapeImg, transform) as GameObject);
        }

        //positions square images to make shape
        foreach(var square in currShape)
        {
            //positions all squares in shape at zero so can be positioned correctly when actually forming shape
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);
        }
        
        var spre = squareShapeImg.GetComponent<SpriteRenderer>();

        var moveDistance = new Vector2(spre.bounds.size.x * squareShapeImg.transform.localScale.x,
            spre.bounds.size.y * squareShapeImg.transform.localScale.y);

        int currListIndex = 0;

        //set square positions to form shape
        for(int row = 0; row < sd.rowCount; row++)
        {
            for(int col = 0; col < sd.columnCount; col++)
            {
                //if loop at active square in shape
                if (sd.shapeBoard[row].column[col])
                {
                    currShape[currListIndex].SetActive(true);
                    currShape[currListIndex].transform.localPosition = new Vector2(GetSquareXPos(sd, col, moveDistance), GetSquareYPos(sd, row, moveDistance));

                    currListIndex++;
                }
            }
        }
    }

    //calculate x and y pos for each square in shape
    private float GetSquareXPos(ShapeData sd, int col, Vector2 moveDistance)
    {
        //allows squares to move horizontally on the grid while maintaining the same shape
        float xShift = 0f;

        //calculates vertical position
        if(sd.columnCount > 1)
        {
            float xStartPos;
            
            //odd number of squares in width
            if (sd.columnCount % 2 != 0)
            {
                xStartPos = (sd.columnCount / 2) * moveDistance.x * -1;
            }

            //even number of squares in width
            else
            {
                xStartPos = ((sd.columnCount / 2) - 1) * moveDistance.x * -1 - moveDistance.x / 2;
            }

            xShift = xStartPos + col * moveDistance.x;
        }

        return xShift;
    }

    private float GetSquareYPos(ShapeData sd, int row, Vector2 moveDistance)
    {
        float yShift = 0f;

        //calculates horizontal position
        if (sd.rowCount > 1)
        {
            float yStartPos;
            
            //odd number of squares in height
            if(sd.rowCount % 2 != 0)
            {
                yStartPos = (sd.rowCount / 2) * moveDistance.y;
            }

            //even number of squares in height
            else
            {
                yStartPos = ((sd.rowCount / 2) - 1) * moveDistance.y + moveDistance.y / 2;
            }

            yShift = yStartPos - row * moveDistance.y;
        }

        return yShift;
    }
    
    //functions required for moving shape in game space
    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.transform.localScale = selectedShapeScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //temp value for shape position
        Vector2 pos = this.transform.position;

        this.transform.position = pos + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameEvents.CheckIfShapePlacable();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }
}
