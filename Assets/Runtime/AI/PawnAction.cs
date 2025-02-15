using MyTool;
using System.Text;
using UnityEngine;

/// <summary>
/// 一个动作
/// </summary>
[System.Serializable]
public class PawnAction : IAnimationSource , IDescription
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
        Debug.Log(ResultDescription);
        if (target != agent.GridObject.CellPosition)
        {
            Vector2Int direction = (Vector2Int)(target - agent.GridObject.CellPosition);
            agent.faceDirection = EDirectionTool.NearestDirection4(direction);
        }
        AnimationProcess animation = skill.MockAnimation(this);
        float time = 0f;
        if (animation != null)
        {
            time = animation.MockTime(this);
            animationManager.Register(animation, 0f);
        }
        effectUnit.Play(animationManager, time);
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

    public void ExtractKeyWords(KeyWordList keyWordList)
    {
        keyWordList.Push(skill.displayName, skill.Description);
    }

    private void Describe(StringBuilder sb, bool result)
    {
        if(result)
        {
            sb.Append(agent.name.Bold());
            sb.Append("使用了");
        }
        sb.Append(skill.displayName);
        sb.AppendLine();
        sb.AppendLine($"时间花费:{Time}");
        for (int i = 0; i < effectUnit.effects.Count; i++)
        {
            effectUnit.effects[i].Describe(sb, result);
        }
    }

    public void Apply()
    {
        
    }
}
