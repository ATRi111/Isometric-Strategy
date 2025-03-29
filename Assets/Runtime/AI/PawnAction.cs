using MyTool;
using Services.Event;
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
    public Vector3Int position;
    public Vector3Int target;
    public EffectUnit effectUnit;
    public int Time => effectUnit.timeEffect.current - effectUnit.timeEffect.prev;
    /// <summary>
    /// 遍历effect，获取第一个不同于agent的PawnVictim
    /// </summary>
    public PawnEntity FirstPawnVictim
    {
        get
        {
            foreach (Effect effect in effectUnit.effects)
            {
                if (effect.PawnVictim != null && effect.PawnVictim != agent)
                    return effect.PawnVictim;
            }
            return null;
        }
    }

    public PawnAction(PawnEntity agent, Skill skill, Vector3Int target)
    {
        this.skill = skill;
        this.target = target;
        this.agent = agent;
        position = agent.GridObject.CellPosition;
    }

    /// <summary>
    /// 模拟动作产生的影响
    /// </summary>
    public void Mock(PawnEntity agent, IsometricGridManager igm)
    {
        effectUnit = new(agent);
        skill.Mock(agent, igm, agent.GridObject.CellPosition, target, effectUnit);
    }

    /// <summary>
    /// 执行并播放动作
    /// </summary>
    public void Play(IEventSystem eventSystem, AnimationManager animationManager)
    {
        eventSystem.Invoke(EEvent.BattleLog, ResultDescription);
        if (target != position)
        {
            Vector2Int direction = (Vector2Int)(target - position);
            agent.faceDirection = EDirectionTool.NearestDirection4(direction);
        }
        AnimationProcess animation = skill.MockAnimation(this);
        skill.PlayMovement(agent.PawnAnimator);
        float latency = 0f;
        if (animation != null)
        {
            latency = animation.MockLatency(this);
            animationManager.Register(animation, 0f);
        }
        effectUnit.Play(animationManager, latency);
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

    }

    private void Describe(StringBuilder sb, bool result)
    {
        if(result)
        {
            sb.Append(agent.EntityName.Bold());
            sb.Append("使用了");
        }
        sb.Append(skill.displayName.Bold());
        sb.AppendLine();
        sb.AppendLine($"时间花费:{Time}");
        for (int i = 0; i < effectUnit.effects.Count; i++)
        {
            if (!result || !effectUnit.effects[i].NeverHappen)
                effectUnit.effects[i].Describe(sb, result);
        }
    }

    public void Apply()
    {
        
    }
}
