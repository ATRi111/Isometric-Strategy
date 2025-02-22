using System;
using UnityEngine;

[Serializable]
public class ShadowVertex
{
    public readonly Vector3Int cellPosition;
    public readonly Vector3Int cellNormal;

    public ShadowVertex(Vector3Int cellPosition, Vector3Int cellNormal)
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
