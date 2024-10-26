using UnityEngine;

/// <summary>
/// 一个动作
/// </summary>
public class PawnAction
{
    public Skill skill;
    public Vector2Int target;

    public PawnAction(Skill skill, Vector2Int target)
    {
        this.skill = skill;
        this.target = target;
    }

    /// <summary>
    /// 模拟动作产生的影响
    /// </summary>
    public void Mock(PawnEntity agent, IsometricGridManager igm, EffectUnit ret)
    {
        skill.Mock(agent, target, igm, ret);
    }
}
