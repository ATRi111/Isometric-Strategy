using EditorExtend.GridEditor;
using UnityEngine;

/// <summary>
/// 在一定范围内释放,且有弹道的技能
/// </summary>
public abstract class ProjectileSkill : RangedSkill
{
    public GridObject HitCheck(PawnEntity agent, IsometricGridManager igm, Vector3Int target, out Vector3 hit)
    {
        GridObject victim = igm.GetObejectXY((Vector2Int)target);
        hit = victim.Center;
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