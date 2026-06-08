using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptySpace : MonoBehaviour
{
    //public Vector2Int start; //maybe not necessary?
    public List<Vector2Int> spacePath; //coordinate +- ints here when checking
    public List<Vector2Int> borderPath; //coordinate +- ints here when checking, first element will always be start.x - 1

    //all bounds border inclusive
    public int xBound; //as always start check from left, how many spaces to the right of start point need to be available for a successful check?
    public int yAboveBound; //how many spaces above the start point need to be availble for a successful check?
    public int yBelowBound; //how many spaces below the start point need to be available for a successful check?
}
