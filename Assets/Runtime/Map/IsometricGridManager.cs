using EditorExtend.GridEditor;
using MyTool;
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

    private readonly List<Vector2Int> overlap = new();
    /// <summary>
    /// 获取第一个与有向线段相交的GridObject(自动忽略与from重合的物体)，并计算第一个交点位置
    /// </summary>
    public GridObject LineSegmentCast(Vector3Int from, Vector3Int to, out Vector3 hit)
    {
        hit = to + GridUtility.CenterOffset;
        List<GridObject> gridObjects = new();
        EDirectionTool.OverlapInt((Vector2Int)from, (Vector2Int)to, overlap);
        if (overlap.Count == 0)
            return null;

        Vector3 enter = from + GridUtility.CenterOffset;
        Vector3 exit = to + GridUtility.CenterOffset;

        bool top_down = (to - from).z < 0;  //射线从上往下发射时，求交时也必须从上往下

        GetObejectsXY(overlap[0], gridObjects, top_down);
        for (int j = 0; j < gridObjects.Count; j++)
        {
            if (gridObjects[j].Overlap(from))
                continue;
            if (gridObjects[j].OverlapLineSegment(ref enter, ref exit))
            {
                hit = enter;
                return gridObjects[j];
            }
        }
        
        for (int i = 1; i < overlap.Count; i++)
        {
            GetObejectsXY(overlap[i], gridObjects, top_down);
            for (int j = 0; j < gridObjects.Count; j++)
            {
                if (gridObjects[j].OverlapLineSegment(ref enter, ref exit))
                {
                    hit = enter;
                    return gridObjects[j];
                }
            }
        }
        return null;
    }
}
