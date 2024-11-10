using Services;
using UnityEngine;

/// <summary>
/// 一个动作
/// </summary>
[System.Serializable]
public class PawnAction
{
    public PawnEntity agent;
    public Skill skill;
    public Vector3Int target;
    public EffectUnit effectUnit;

    public PawnAction(PawnEntity agent, Skill skill, Vector3Int target)
    {
        this.skill = skill;
        this.target = target;
        this.agent = agent;
        effectUnit = new(agent);
    }

    /// <summary>
    /// 模拟动作产生的影响
    /// </summary>
    public void Mock(PawnEntity agent, IsometricGridManager igm)
    {
        skill.Mock(agent, igm, agent.GridObject.CellPosition, target, effectUnit);
    }

    /// <summary>
    /// 执行动作
    /// </summary>
    public void Excute()
    {
        effectUnit.Play();
        ServiceLocator.Get<AnimationManager>().StartAnimationCheck();
    }
}
