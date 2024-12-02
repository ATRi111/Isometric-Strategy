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
        return GameObject.Find(nameof(IsometricGridManager)).GetComponent<IsometricGridManager>();
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
    /// ��ȡ��һ���������߶��ཻ��GridObject(�Զ�������from�غϵ�����)���������һ������λ��
    /// </summary>
    public GridObject LineSegmentCast(Vector3 from, Vector3 to, out Vector3 hit)
    {
        hit = from;
        List<GridObject> gridObjects = new();
        Vector2Int ifrom = new(Mathf.FloorToInt(from.x), Mathf.FloorToInt(from.y));
        Vector2Int ito = new(Mathf.FloorToInt(to.x), Mathf.FloorToInt(to.y));
        EDirectionTool.OverlapInt(ifrom, ito, overlap);
        if (overlap.Count == 0)
            return null;

        bool top_down = (to - from).z < 0;  //���ߴ������·���ʱ����ʱҲ�����������

        GetObjectsXY(overlap[0], gridObjects, top_down);
        for (int j = 0; j < gridObjects.Count; j++)
        {
            if (gridObjects[j].Overlap(from))
                continue;
            if (gridObjects[j].OverlapLineSegment(ref from, ref to))
            {
                hit = from;
                return gridObjects[j];
            }
        }
        
        for (int i = 1; i < overlap.Count; i++)
        {
            GetObjectsXY(overlap[i], gridObjects, top_down);
            for (int j = 0; j < gridObjects.Count; j++)
            {
                if (gridObjects[j].OverlapLineSegment(ref from, ref to))
                {
                    hit = from;
                    return gridObjects[j];
                }
            }
        }
        return null;
    }

    /// <summary>
    /// ��ȡ��һ���������������ཻ��GridObject(�Զ�������from�غϵ�����)���������һ������λ��
    /// </summary>
    public GridObject ParabolaCast(Vector3 from, Vector3 velocity, float g, float time, out Vector3 hit)
    {
        hit = from;
        List<Vector3> vs = new();
        GridPhysics.DiscretizeParabola(from, velocity, g, time, GridPhysics.settings.parabolaPrecision, vs);
        List<GridObject> gridObjects = new();
        for (int i = 1; i < vs.Count - 1; i++)  //�����������յ�
        {
            Vector2Int xy = new(Mathf.FloorToInt(vs[i].x), Mathf.FloorToInt(vs[i].y));
            GetObjectsXY(xy, gridObjects);
            for (int j = 0; j < gridObjects.Count; j++)
            {
                if (gridObjects[j].Overlap(vs[i]) && !gridObjects[j].Overlap(from))
                {
                    hit = vs[i];
                    return gridObjects[j];
                }
            }
        }
        return null;
    }
}
