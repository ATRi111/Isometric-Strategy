using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 在一定范围内释放,且有弹道的技能
/// </summary>
public abstract class ProjectileSkill : RangedSkill
{
    //应当在Mock时更新此
    public readonly List<Vector3> currentTrajectory = new();

    public override void GetVictims(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Entity> ret)
    {
        ret.Clear();
        List<Vector3> trajectory = new();
        GridObject gridObject = HitCheck(agent, igm, target, trajectory);
        if(gridObject != null)
        {
            Entity entity = gridObject.GetComponentInParent<Entity>();
            if (entity != null && FilterVictim(entity))
                ret.Add(entity);
        }
    }

    /// <summary>
    /// 获取瞄准点
    /// </summary>
    /// <returns>瞄准目标是否存在</returns>
    public virtual bool GetAimPoint(IsometricGridManager igm, Vector3Int target, out Vector3 to)
    {
        GridObject aimedObject = igm.GetObjectXY((Vector2Int)target);
        if (aimedObject == null)
        {
            to = target + 0.5f * Vector3.one;
            return false;
        }
        to = aimedObject.GetComponent<Entity>() != null ? aimedObject.Center : aimedObject.TopCenter;
        return true;
    }

    /// <summary>
    /// 计算技能将会命中的目标，并计算弹道
    /// </summary>
    public GridObject HitCheck(PawnEntity agent, IsometricGridManager igm, Vector3Int target, List<Vector3> trajectory)
    {
        trajectory?.Clear();
        if (GetAimPoint(igm, target, out Vector3 to))
        {
            Vector3 from = agent.GridObject.Center;
            GridObject gridObject = HitCheck(igm, from, to, target, trajectory);
            return gridObject;
        }
        return null;
    }

    /// <summary>
    /// 计算技能将会命中的目标，并计算弹道
    /// </summary>
    public abstract GridObject HitCheck(IsometricGridManager igm, Vector3 from, Vector3 to, Vector3Int target, List<Vector3> trajectory);
}