using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptySpace : MonoBehaviour
{
    //public Vector2Int start; //maybe not necessary?
    public List<Vector2Int> spacePath; //coordinate +- ints here when checking
    public List<Vector2Int> borderPath; //coordinate +- ints here when checking, first element will always be start.x - 1
}
