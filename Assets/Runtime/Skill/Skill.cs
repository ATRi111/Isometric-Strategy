using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class Skill : ScriptableObject , IDescription
{
    public string displayName;
    public Sprite icon;

    public string extraDescription;
    public int actionTime;
    public GameObject animationPrefab;
    public EPawnAnimationState pawnAnimationState;
    public float movementLatency;
    public bool weaponAnimation;

    public List<ParameterPreCondition> preConditions;
    public List<BuffPreCondition> buffPreConditions;
    public List<SkillPrecondition> specialPreconditions;
    public List<PawnParameterModifier> parameterOnAgent;
    
    public List<BuffModifier> buffOnAgent;
    public int HPCost;

    public virtual bool Offensive => false;

    /// <summary>
    /// 判断当前能否使用此技能
    /// </summary>
    public virtual bool CanUse(PawnEntity agent, IsometricGridManager igm)
    {
        for (int i = 0;i < preConditions.Count;i++)
        {
            if (!preConditions[i].Verify(agent))
                return false;
        }
        for (int i = 0; i < buffPreConditions.Count; i++)
        {
            if (!buffPreConditions[i].Verify(agent))
                return false;
        }
        for (int i = 0; i < specialPreconditions.Count; i++)
        {
            if (!specialPreconditions[i].Verify(igm, agent))
                return false;
        }
        return true;
    }

    /// <summary>
    /// 获取可选的技能施放位置
    /// </summary>
    public virtual void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        ret.Clear();
    }

    /// <summary>
    /// 模拟技能产生的影响
    /// </summary>
    public virtual void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        ret.timeEffect.current += MockTime(agent, igm, position, target);
        HashSet<string> parameterToReset = PawnEntity.ParameterTable.GetResetParameters();
        for (int i = 0; i < parameterOnAgent.Count; i++)
        {
            PawnParameterModifier modifier = parameterOnAgent[i];
            int value = agent.parameterDict[modifier.ParameterName];
            ModifyParameterEffect effect = new(agent, modifier.ParameterName, value, value + modifier.deltaValue);
            ret.effects.Add(effect);
            parameterToReset.Remove(modifier.ParameterName);    //不会重置技能影响的参数
        }
        foreach (string parameterName in parameterToReset)
        {
            int value = agent.parameterDict[parameterName];
            if (value != 0)
            {
                ModifyParameterEffect effect = new(agent, parameterName, value, 0)
                {
                    reset = true
                };
                ret.effects.Add(effect);   //仅计算效果，尚未应用效果，因此不会影响之后的伤害计算等
            }
        }
        for (int i = 0; i < buffOnAgent.Count; i++)
        {
            BuffEffect buffEffect;
            if (!buffOnAgent[i].remove)
                buffEffect = agent.BuffManager.MockAdd(buffOnAgent[i].so, buffOnAgent[i].probability);
            else
                buffEffect = agent.BuffManager.MockRemove(buffOnAgent[i].so, buffOnAgent[i].probability);

            if (buffEffect != null)
            {
                if(!buffEffect.AlwaysHappen)
                    buffEffect.randomValue = Effect.NextInt();
                ret.effects.Add(buffEffect);
            }
        }
        if(HPCost > 0)
        {
            int hp = agent.DefenceComponent.HP;
            int newHP = Mathf.Max(hp - HPCost, 0);
            HPChangeEffect effect = new(agent, hp, newHP);
            ret.effects.Add(effect);
        }
    }

    /// <summary>
    /// 模拟技能花费的时间
    /// </summary>
    public virtual int MockTime(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target)
    {
        return Mathf.RoundToInt((1f - agent.speedUpRate.CurrentValue) * MockPrimitiveTime(agent, igm, position, target));
    }
    /// <summary>
    /// 模拟技能花费的原始时间
    /// </summary>
    protected virtual int MockPrimitiveTime(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target)
    {
        return actionTime;
    }

    /// <summary>
    /// 模拟技能动画
    /// </summary>
    public virtual AnimationProcess MockAnimation(PawnAction action)
    {
        if (animationPrefab != null)
        {
            ObjectAnimationProcess animationProcess = new(
                action, animationPrefab.name,
                action.agent.transform,
                action.agent.transform.position);
            return animationProcess;
        }
        return null;
    }

    /// <summary>
    /// 播放人物动作
    /// </summary>
    public virtual void PlayMovement(PawnAnimator pawnAnimator)
    {
        if (pawnAnimator != null && pawnAnimationState != EPawnAnimationState.Walk)
            pawnAnimator.Play(pawnAnimationState.ToString(), movementLatency, weaponAnimation);
    }

    /// <summary>
    /// 获取此技能涉及的参数名
    /// </summary>
    public virtual void GetRelatedParameters(HashSet<string> ret)
    {
        for (int i = 0; i < parameterOnAgent.Count; i++)
        {
            ret.Add(parameterOnAgent[i].ParameterName);
        }
        for (int i = 0; i < preConditions.Count; i++)
        {
            ret.Add(preConditions[i].ParameterName);
        }
    }

    #region 描述

    public virtual void ExtractKeyWords(KeyWordList keyWordList)
    {
        for (int i = 0;i < preConditions.Count;i++)
        {
            Parameter p = PawnEntity.ParameterTable[preConditions[i].ParameterName];
            keyWordList.Push(p.name, p.description);
        }
        for (int i = 0; i < buffPreConditions.Count; i++)
        {
            BuffSO so = buffPreConditions[i].so;
            keyWordList.Push(so.name, so.Description);
        }
        for (int i = 0; i < parameterOnAgent.Count; i++)
        {
            Parameter p = PawnEntity.ParameterTable[parameterOnAgent[i].ParameterName];
            keyWordList.Push(p.name, p.description);
        }
        for (int i = 0; i < buffOnAgent.Count; i++)
        {
            BuffSO so = buffOnAgent[i].so;
            keyWordList.Push(so.name, so.Description);
        }
    }

    private string description;
    public string Description
    {
        get
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                description = null;
#endif
            if (string.IsNullOrEmpty(description))
            {
                StringBuilder sb = new();
                Describe(sb);
                sb.Append(extraDescription);
                description = sb.ToString();
            }
            return description;
        }
    }

    protected virtual void Describe(StringBuilder sb)
    {
        DescribeTime(sb);
        if (preConditions.Count + buffPreConditions.Count + specialPreconditions.Count > 0)
            DescribePreConditions(sb);
        if (HPCost != 0)
            DescribeHPCost(sb);
        if (buffOnAgent.Count > 0)
            DescribeBuffOnAgent(sb);
        if(parameterOnAgent.Count > 0)
            DescribeParameterOnAgent(sb);
    }

    protected virtual void DescribePreConditions(StringBuilder sb)
    {
        sb.AppendLine("施放条件：");
        for (int i = 0; i < preConditions.Count; i++)
        {
            preConditions[i].Describe(sb);
        }
        for (int i = 0; i < buffPreConditions.Count; i++)
        {
            buffPreConditions[i].Describe(sb);
        }
        for (int i = 0; i < specialPreconditions.Count; i++)
        {
            if (specialPreconditions[i] != null)
                specialPreconditions[i].Describe(sb);
        }
        sb.AppendLine();
    }

    protected virtual void DescribeTime(StringBuilder sb)
    {
        sb.Append("基本时间消耗：");
        sb.Append(actionTime);
        sb.AppendLine();
    }

    protected virtual void DescribeHPCost(StringBuilder sb)
    {
        sb.Append("消耗自身");
        sb.Append(HPCost);
        sb.Append("点生命");
        sb.AppendLine();
    }

    protected virtual void DescribeBuffOnAgent(StringBuilder sb)
    {
        for (int i = 0; i < buffOnAgent.Count; i++)
        {
            buffOnAgent[i].Describe(sb, "自身");
        }
    }

    protected virtual void DescribeParameterOnAgent(StringBuilder sb)
    {
        for (int i = 0; i < parameterOnAgent.Count; i++)
        {
            parameterOnAgent[i].Describe(sb, "自身");
        }
    }

    #endregion
}
