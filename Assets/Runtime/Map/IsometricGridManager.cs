using EditorExtend.GridEditor;
using MyTool;
using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-500)]
[RequireComponent(typeof(PerspectiveManager))]
public class IsometricGridManager : IsometricGridManagerBase
{
    private static IsometricGridManager instance;
    public static IsometricGridManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = GameObject.Find(nameof(IsometricGridManager));
                if (obj != null)
                    instance = obj.GetComponent<IsometricGridManager>();
            }
            return instance;
        }
    }

    private readonly Dictionary<Vector3Int, Entity> entityDict = new();
    public Dictionary<Vector3Int,Entity> EntityDict => entityDict;

    public override void Clear()
    {
        base.Clear();
        entityDict.Clear();
    }

    public override bool TryAddObject(GridObject gridObject)
    {
        if (!base.TryAddObject(gridObject))
            return false;

        if (gridObject.TryGetComponent<Entity>(out var entity))
            entityDict.Add(gridObject.CellPosition, entity);
        return true;
    }

    public override GridObject TryRemoveObject(Vector3Int cellPosition)
    {
        GridObject gridObject = base.TryRemoveObject(cellPosition);
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
        hit = to;
        List<GridObject> gridObjects = new();
        Vector2Int ifrom = new(Mathf.FloorToInt(from.x), Mathf.FloorToInt(from.y));
        Vector2Int ito = new(Mathf.FloorToInt(to.x), Mathf.FloorToInt(to.y));
        EDirectionTool.OverlapInt(ifrom, ito, overlap);
        if (overlap.Count == 0)
            return null;

        bool top_down = (to - from).z < 0;  //���ߴ������·���ʱ����ʱҲ�����������
        for (int i = 0; i < overlap.Count; i++)
        {
            GetObjectsXY(overlap[i], gridObjects, top_down); 
            for (int j = 0; j < gridObjects.Count; j++)
            {
                if (!gridObjects[j].Overlap(from) && gridObjects[j].OverlapLineSegment(ref from, ref to))
                {
                    hit = from;
                    return gridObjects[j];
                }
            }
        }
        return null;
    }

    /// <summary>
    /// ��ȡ��һ���������������ཻ��GridObject(�Զ�������from�غϵ�����)��������켣
    /// </summary>
    public GridObject ParabolaCast(Vector3 from, Vector3 velocity, float g, List<Vector3> trajectory)
    {
        if (g <= 0 || velocity == Vector3.zero)
            throw new ArgumentException();
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
                throw new Exception();
            if (point.z < 0)
                break;
            Vector2Int xy = new(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y));
            GetObjectsXY(xy, gridObjects);
            for (int j = 0; j < gridObjects.Count; j++)
            {
                if (!gridObjects[j].Overlap(from) && gridObjects[j].Overlap(point))
                    return gridObjects[j];
            }
        }
        return null;
    }

    /// <summary>
    /// ��ȡ��һ���������������ཻ��GridObject(�Զ�������from�غϵ�����)
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
    /// ��ȡ��һ���������������ཻ,�ҷ�������������GridObject(�Զ�������from�غϵ�����)
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
