using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeStorage : MonoBehaviour
{
    public List<ShapeData> shapeDataList;
    public List<Shape> shapeList;

    private float[] rotationsList = {0.0f, 90.0f, 180.0f, 270.0f}; 
    
    void Start()
    {
        foreach(var shape in shapeList)
        {
            //get index of random shape
            int shapeIndex = UnityEngine.Random.Range(0, shapeDataList.Count);

            shape.CreateShape(shapeDataList[shapeIndex]);

            float rotation = rotationsList[UnityEngine.Random.Range(0, rotationsList.Length - 1)];


            shape.GetComponent<RectTransform>().transform.Rotate(0.0f, 0.0f, rotation, Space.Self);
            Debug.Log("setting shape rotation to " + rotation);
        }
    }

}
