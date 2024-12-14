using EditorExtend.GridEditor;
using MyTool;
using System;
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
    /// 获取第一个与有向线段相交的GridObject(自动忽略与from重合的物体)，并计算第一个交点位置
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

        bool top_down = (to - from).z < 0;  //射线从上往下发射时，求交时也必须从上往下

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
    /// 获取第一个与有向抛物线相交的GridObject(自动忽略与from重合的物体)，并计算轨迹
    /// </summary>
    public GridObject ParabolaCast(Vector3 from, Vector3 velocity, float g, List<Vector3> trajectory)
    {
        if (g <= 0 || velocity == Vector3.zero)
            throw new System.ArgumentException();
        if (trajectory == null)
            return ParabolaCast(from, velocity, g);

        trajectory.Clear();

        List<GridObject> gridObjects = new();
        float deltaTime = Mathf.Clamp(1f / velocity.magnitude / GridPhysics.settings.parabolaPrecision, 0.01f, 0.1f);
        for (float t = 0f; ;t += deltaTime)
        {
            Vector3 point = from + t * velocity + t * t / 2 * g * Vector3.back;
            trajectory.Add(point);
            if(trajectory.Count > 1000)
                throw new System.Exception();
            if (point.z < 0)
                break;
            Vector2Int xy = new(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y));
            GetObjectsXY(xy, gridObjects);
            for (int j = 0; j < gridObjects.Count; j++)
            {
                if (gridObjects[j].Overlap(point) && !gridObjects[j].Overlap(from))
                    return gridObjects[j];
            }
        }
        return null;
    }

    /// <summary>
    /// 获取第一个与有向抛物线相交的GridObject(自动忽略与from重合的物体)
    /// </summary>
    public GridObject ParabolaCast(Vector3 from, Vector3 velocity, float g)
    {
        if (g <= 0)
            throw new System.ArgumentException();
        List<GridObject> gridObjects = new();
        float deltaTime = Mathf.Max(1f / velocity.magnitude / GridPhysics.settings.parabolaPrecision, 0.01f);
        for (float t = 0f; ; t += deltaTime)
        {
            Vector3 point = from + t * velocity + t * t / 2 * g * Vector3.back;
            if (point.z < 0)
                break;
            Vector2Int xy = new(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y));
            GetObjectsXY(xy, gridObjects);
            for (int j = 0; j < gridObjects.Count; j++)
            {
                if (gridObjects[j].Overlap(point) && !gridObjects[j].Overlap(from))
                    return gridObjects[j];
            }
        }
        return null;
    }

    // <summary>
    /// 获取第一个与有向抛物线相交,且符合所给条件的GridObject(自动忽略与from重合的物体)
    /// </summary>
    public GridObject ParabolaCast(Vector3 from, Vector3 velocity, float g, PawnEntity pawn, Func<PawnEntity, GridObject, bool> ObjectCheck)
    {
        if (g <= 0)
            throw new ArgumentException();
        List<GridObject> gridObjects = new();
        float deltaTime = Mathf.Max(1f / velocity.magnitude / GridPhysics.settings.parabolaPrecision, 0.01f);
        for (float t = 0f; ; t += deltaTime)
        {
            Vector3 point = from + t * velocity + t * t / 2 * g * Vector3.back;
            if (point.z < 0)
                break;
            Vector2Int xy = new(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y));
            GetObjectsXY(xy, gridObjects);
            for (int j = 0; j < gridObjects.Count; j++)
            {
                if (gridObjects[j].Overlap(point) && !gridObjects[j].Overlap(from) && ObjectCheck.Invoke(pawn, gridObjects[j]))
                    return gridObjects[j];
            }
        }
        return null;
    }
}
