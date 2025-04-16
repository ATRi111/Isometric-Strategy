using EditorExtend.GridEditor;
using MyTool;
using Services;
using Services.Event;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-500)]
[RequireComponent(typeof(PerspectiveManager), typeof(BattleField), typeof(ShadowManager))]
public class IsometricGridManager : IsometricGridManagerBase
{
    public static Vector3Int CoverVector = new(1, 1, -2);
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

    public BattleField BattleField { get; private set; }

    #region 物体管理
    public readonly Dictionary<Vector3Int, Entity> entityDict = new();

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
    #endregion

    #region 物理系统
    private readonly List<Vector2Int> overlap = new();
    /// <summary>
    /// 获取第一个与有向线段相交的GridObject(自动忽略与from重合的物体)，并计算第一个交点位置
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

        bool top_down = (to - from).z < 0;  //射线从上往下发射时，求交时也必须从上往下
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
    /// 获取第一个与有向抛物线相交的GridObject(自动忽略与from重合的物体)，并计算轨迹
    /// </summary>
    /// <param name="from">抛物线起点</param>
    /// <param name="velocity">抛物线初速度</param>
    /// <param name="g">重力加速度（沿Z轴负方向为正）</param>
    /// <param name="trajectory">接收轨迹</param>
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
    /// 获取第一个与有向抛物线相交的GridObject(自动忽略与from重合的物体)
    /// </summary>
    /// <param name="from">抛物线起点</param>
    /// <param name="velocity">抛物线初速度</param>
    /// <param name="g">重力加速度（沿Z轴负方向为正）</param>
    public GridObject ParabolaCast(Vector3 from, Vector3 velocity, float g)
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
    #endregion

    #region 攀爬

    private readonly HashSet<Ladder> ladders = new();

    public bool ClimbCheck(Vector3Int from, Vector3Int to)
    {
        Vector3Int climbDirection, ladderDirection, current;
        if(to.z > from.z)
        {
            climbDirection = Vector3Int.forward;
            current = to.ResetZ(from.z) + Vector3Int.forward;
            ladderDirection = (from - to).ResetZ();
        }
        else
        {
            climbDirection = Vector3Int.back;
            current = from + Vector3Int.back;
            ladderDirection = (to - from).ResetZ();
        }
        for (; ; current += climbDirection)
        {
            if (current.z == to.z)
                return true;
            Ladder ladder = new()
            {
                cellPosition = current,
                direction = ladderDirection
            };
            if (!ladders.Contains(ladder))
                return false;
        }
    }

    #endregion

#if UNITY_EDITOR

    #region 透视
    public int sortingOrderThreshold;   //sortingOrder小于等于此值的物体可见

    public void ModifySortingOrderThreshold(int deltaValue)
    {
        sortingOrderThreshold += deltaValue;
        Perspect();
    }

    public void ResetSortingOrderThreshold()
    {
        int max = int.MinValue;
        List<GridObject> gridObjects = new();
        GeneralTool.GetComponentsInAllChildren(transform, gridObjects);
        for (int i = 0; i < gridObjects.Count; i++)
        {
            GridObject gridObject = gridObjects[i];
            gridObject.gameObject.SetActive(true);
            max = Mathf.Max(max, CellToSortingOrder(gridObject.transform.position));
        }
        sortingOrderThreshold = max;
        Perspect();
    }

    public void Perspect()
    {
        List<GridObject> gridObjects = new();
        GeneralTool.GetComponentsInAllChildren(transform, gridObjects);
        for(int i = 0; i < gridObjects.Count; i++)
        {
            GridObject gridObject = gridObjects[i];
            gridObject.gameObject.SetActive(CellToSortingOrder(gridObject.transform.position) <= sortingOrderThreshold);
        }
        AddAllObjects();
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }
    #endregion

#endif

    private void Awake()
    {
        BattleField = GetComponentInChildren<BattleField>();
    }

    private void Start()
    {
        ServiceLocator.Get<IEventSystem>().Invoke(EEvent.AfterMapInitialize);
        GridSurface[] gridSurfaces = GetComponentsInChildren<GridSurface>();
        for (int i = 0; i < gridSurfaces.Length; i++)
        {
            gridSurfaces[i].GetLadder(ladders);
        }
    }
}
