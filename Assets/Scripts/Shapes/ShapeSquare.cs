using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ShapeSquare : MonoBehaviour
{
    public Image occupiedImg;

    void Start()
    {
        occupiedImg.gameObject.SetActive(false); 
    }
}
