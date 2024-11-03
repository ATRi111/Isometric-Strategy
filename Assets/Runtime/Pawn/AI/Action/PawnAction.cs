using UnityEngine;

/// <summary>
/// 一个动作
/// </summary>
public class PawnAction
{
    public PawnEntity agent;
    public Skill skill;
    public Vector2Int target;
    public EffectUnit effectUnit;

    public PawnAction(PawnEntity agent, Skill skill, Vector2Int target)
    {
        this.skill = skill;
        this.target = target;
        effectUnit = new(agent);
    }

    /// <summary>
    /// 模拟动作产生的影响
    /// </summary>
    public void Mock(PawnEntity agent, IsometricGridManager igm)
    {
        skill.Mock(agent, igm, (Vector2Int)agent.GridObject.CellPosition, target, effectUnit);
    }

    /// <summary>
    /// 执行动作
    /// </summary>
    public void Excute()
    {
        effectUnit.Play();
    }
}
