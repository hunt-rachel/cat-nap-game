using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    public GameObject testShape;

    void OnMouseDown()
    {
        Vector3 pos = testShape.transform.position;
        
        screenPoint = Camera.main.WorldToScreenPoint(pos);
        offset = pos - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
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

    //help with snapping to grid
    void OnMouseUp()
    {
        Vector3 shapePos = testShape.transform.position;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(shapePos);

        int snapX = Mathf.RoundToInt(shapePos.x);
        int snapY = Mathf.RoundToInt(shapePos.y);

        testShape.transform.position = new Vector3(snapX, snapY, screenPoint.z);
    }
}
