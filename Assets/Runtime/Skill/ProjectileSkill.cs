using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 在一定范围内释放,且有弹道的技能
/// </summary>
public abstract class ProjectileSkill : RangedSkill
{
    public override void GetVictims(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Entity> ret)
    {
        ret.Clear();
        GridObject gridObject = HitCheck(agent, igm, target, out Vector3 _);
        if(gridObject != null)
        {
            Entity entity = gridObject.GetComponentInParent<Entity>();
            if (entity != null && FilterVictim(entity))
                ret.Add(entity);
        }
    }

    public GridObject HitCheck(PawnEntity agent, IsometricGridManager igm, Vector3Int target, out Vector3 hit)
    {
        GridObject victim = igm.GetObjectXY((Vector2Int)target);
        hit = Vector3.zero;
        if (victim == null)
            return null;
        if (victim != null)
        {
            Vector3 from = agent.GridObject.Center;
            Vector3 to = victim.GetComponent<Entity>() != null ? victim.Center : victim.TopCenter;
            GridObject gridObject = HitCheck(igm, from, to, out hit);
            return gridObject;
        }
        return null;
    }

    public abstract GridObject HitCheck(IsometricGridManager igm, Vector3 from, Vector3 to, out Vector3 hit);
}