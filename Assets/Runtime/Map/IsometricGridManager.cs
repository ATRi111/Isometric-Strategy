using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

public class IsometricGridManager : IsometricGridManagerBase
{
    private readonly Dictionary<Vector3Int, Entity> entityDict = new();
    public Dictionary<Vector3Int,Entity> EntityDict => entityDict;

    public static IsometricGridManager FindInstance()
    {
        return GameObject.Find("Grid").GetComponent<IsometricGridManager>();
    }

    public override void Clear()
    {
        base.Clear();
        entityDict.Clear();
    }

    public override void AddObject(GridObject gridObject)
    {
        base.AddObject(gridObject);
        Entity entity = gridObject.GetComponentInParent<Entity>();
        if (entity != null)
            entityDict.Add(gridObject.CellPosition, entity);
    }

    public override GridObject RemoveObject(Vector3Int cellPosition)
    {
        GridObject gridObject = base.RemoveObject(cellPosition);
        if(gridObject is MovableGridObject)
        {
            entityDict.Remove(cellPosition);
        }
        return gridObject;
    }
}
