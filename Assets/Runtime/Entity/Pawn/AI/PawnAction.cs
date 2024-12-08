using System.Text;
using UnityEngine;

/// <summary>
/// һ������
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
    /// ģ�⶯��������Ӱ��
    /// </summary>
    public void Mock(PawnEntity agent, IsometricGridManager igm)
    {
        skill.Mock(agent, igm, agent.GridObject.CellPosition, target, effectUnit);
    }

    /// <summary>
    /// ִ�в����Ŷ���
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
        sb.AppendLine($"ʱ�仨��:{Time}");
        return sb.ToString();
    }
}
