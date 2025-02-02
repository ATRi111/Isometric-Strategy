using MyTool;
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
        Debug.Log(ResultDescription);
        if (target != agent.GridObject.CellPosition)
        {
            Vector2Int direction = (Vector2Int)(target - agent.GridObject.CellPosition);
            agent.faceDirection = EDirectionTool.NearestDirection4(direction);
        }
        AnimationProcess animation = skill.GenerateAnimation(out float time);
        if (animation != null)
            animationManager.Register(animation, 0f);
        effectUnit.Play(animationManager, time);
        animationManager.StartAnimationCheck();
    }

    public string Description
    {
        get
        {
            StringBuilder sb = new();
            Describe(sb, false);
            return sb.ToString();
        }
    }

    public string ResultDescription
    {
        get
        {
            StringBuilder sb = new();
            Describe(sb, true);
            return sb.ToString();
        }
    }

    private void Describe(StringBuilder sb, bool result)
    {
        if(result)
        {
            sb.Append(agent.name.Bold());
            sb.Append("ʹ����");
        }
        sb.Append(skill.displayName.Bold());
        sb.AppendLine();
        sb.AppendLine($"ʱ�仨��:{Time}");
        for (int i = 0; i < effectUnit.effects.Count; i++)
        {
            effectUnit.effects[i].Describe(sb, result);
        }
    }
}
