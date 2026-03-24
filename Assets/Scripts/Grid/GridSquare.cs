using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    public Image squareImg;

    public List<Sprite> squareImages;

    public void SetImage(bool setFirstImg)
    {
        squareImg.GetComponent<Image>().sprite = setFirstImg ? squareImages[1] : squareImages[0];
    }

}