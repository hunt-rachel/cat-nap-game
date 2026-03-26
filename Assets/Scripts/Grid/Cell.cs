using UnityEngine;

public struct Cell
{
    public enum Type
    {
        Edge,
        Empty,
        Occupied,
        Hovering_CanPlace,
        Hovering_CannotPlace,
    }

    public Vector3Int pos;
    public Type type;
}