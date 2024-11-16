using EditorExtend.GridEditor;
using UnityEngine;

/// <summary>
/// 在一定范围内释放,且有弹道的技能
/// </summary>
public abstract class ProjectileSkill : RangedSkill
{
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
