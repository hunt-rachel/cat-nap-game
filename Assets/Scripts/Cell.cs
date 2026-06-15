using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct Cell
{
    public enum Type
    {
        Empty,
        Filled,
        Edge,
        Invalid,
    }

    public Vector3Int pos;
    public Type type;
    public Color colour;

    public bool filled;
    public bool isEdge;
}
