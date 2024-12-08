using System.Text;
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
    public int Time => effectUnit.timeEffect.current - effectUnit.timeEffect.prev;

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
    /// 执行并播放动作
    /// </summary>
    public void Play(AnimationManager animationManager)
    {
        AnimationProcess animation = skill.GenerateAnimation();
        if (animation != null)
            animationManager.Register(animation);
        effectUnit.Play(animationManager);
        animationManager.StartAnimationCheck();
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine(skill.name);
        sb.AppendLine($"时间花费:{Time}");
        return sb.ToString();
    }
}
