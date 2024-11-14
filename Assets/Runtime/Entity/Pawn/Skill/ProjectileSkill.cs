using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 有弹道的技能
/// </summary>
public abstract class ProjectileSkill : Skill
{
    protected readonly Dictionary<Vector3Int, Vector3Int> cache_HitPoint = new();

    public virtual GridObject HitCheck(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target)
    {
        GridObject victim = igm.GetObejectXY((Vector2Int)position);
        if (victim != null)
        {
            GridObject gridObject = HitCheck(igm, agent.GridObject.Center, victim.Center, out Vector3 hit);
            return gridObject;
        }
        return null;
    }

    public abstract GridObject HitCheck(IsometricGridManager igm, Vector3 from, Vector3 to, out Vector3 hit);
}
