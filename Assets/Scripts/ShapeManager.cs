using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShapeManager : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    private Board board;
    private GameManager gm;

    public GameObject testShape;

    public Vector3Int testStartPos;

    private void Awake()
    {
        board = GameObject.Find("Game Board Grid").GetComponentInChildren<Board>();
        //if (board) { Debug.Log("found the board"); }
        gm = GameObject.Find("Game Board Grid").GetComponent<GameManager>();
        //if (gm) { Debug.Log("found the game manager script"); }
    }

    void OnMouseDown()
    {
        Vector3 pos = testShape.transform.position;
        
        screenPoint = Camera.main.WorldToScreenPoint(pos);
        offset = pos - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        testStartPos = new Vector3Int(3, 0, 0);
    }

    void OnMouseDrag()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 currPoint = new Vector3(mousePos.x, mousePos.y, screenPoint.z);

        Vector3 currPos = Camera.main.ScreenToWorldPoint(currPoint) + offset;

        //maybe remove depending on if it works well
        if(Input.GetMouseButtonUp(1))
        {
            testShape.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);

            //reset rotation value if done full spin
            if(testShape.transform.rotation.eulerAngles.z >= 360.0f)
            {
                testShape.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
        }

        testShape.transform.position = currPos;
    }


    void OnMouseUp()
    {
        PlaceShape();
    }

    private void PlaceShape()
    {
        Vector3 shapePos = testShape.transform.position;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(shapePos);

        bool canPlaceShape = true;
        
        //check validity of each child (individual square) of shape
        for(int i = 0; i < testShape.transform.childCount; i++)
        {
            Transform currChild = testShape.transform.GetChild(i);
            int childXPos = Mathf.RoundToInt(currChild.position.x);
            int childYPos = Mathf.RoundToInt(currChild.position.y);

            //get reference to current cell with correct indices for state 2D array
            int currStateXRef = Math.Abs(childXPos + gm.stateRefXOffset);
            int currStateYRef = Math.Abs(childYPos + gm.stateRefYOffset);

            Cell currCell = gm.state[currStateXRef,currStateYRef];

            //Debug.Log("current cell pos is: x: " + childXPos + ", y: " + childYPos);
            //Debug.Log("referece in state should be: " + Math.Abs(childXPos + gm.stateRefXOffset) + ", " + Math.Abs(childYPos + gm.stateRefYOffset));

            //Debug.Log("curr cell type is: " + currCell.type);

            //player tried to place shape in invalid space
            if(currCell.type != Cell.Type.Empty)
            {
                canPlaceShape = false;
                break;
            }

        }

        if(canPlaceShape)
        {
            //help with snapping to grid
            int snapX = Mathf.RoundToInt(shapePos.x);
            int snapY = Mathf.RoundToInt(shapePos.y);

            testShape.transform.position = new Vector3(snapX, snapY, screenPoint.z);
        }

        else
        {
            testShape.transform.position = testStartPos;
        }
    }
}
