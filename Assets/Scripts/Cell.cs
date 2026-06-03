using UnityEngine;

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

    public bool filled;
}
