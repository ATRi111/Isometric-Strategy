using System;
using UnityEngine;

[Serializable]
public class ShadowVertex
{
    public Vector3 cellPosition;
    public Vector3 cellNormal;

    public ShadowVertex(Vector3 cellPosition, Vector3 cellNormal)
    {
        this.cellPosition = cellPosition;
        this.cellNormal = cellNormal;
    }

    public override bool Equals(object obj)
    {
        if(obj is ShadowVertex vertex)
        {
            return cellPosition == vertex.cellPosition && cellNormal == vertex.cellNormal;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(cellPosition, cellNormal);
    }
}
