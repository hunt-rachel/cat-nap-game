using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    //public vars
    public Image defaultImg;
    public Image hoverImg;
    public Image activeImg;

    public List<Sprite> defaultImages;

    public bool Selected { get; set; }
    public bool SquareOccupied { get; set; }

    public int SquareIndex { get; set; }

    void Start()
    {
        Selected = false;
        SquareOccupied = false;  
    }

    //temp testing function
    public bool PlacableHere()
    {
        return hoverImg.gameObject.activeSelf;
    }

    public void ActivateSquare()
    {
        hoverImg.gameObject.SetActive(false);
        activeImg.gameObject.SetActive(true);

        Selected = true;
        SquareOccupied = true;
    }
    
    public void SetImage(bool setFirstImg)
    {
        defaultImg.GetComponent<Image>().sprite = setFirstImg ? defaultImages[1] : defaultImages[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        hoverImg.gameObject.SetActive(true);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        hoverImg.gameObject.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        hoverImg.gameObject.SetActive(false);
    }

}