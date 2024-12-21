using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public string displayName;
    public int actionTime;
    public List<SkillPreCondition> preConditions;
    public List<PawnParameterModifier> parameterModifiers;
    public List<BuffModifier> buffOnAgent;

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
        HashSet<string> parameterToReset = PawnEntity.ParameterTable.resetParameters.ToHashSet();
        for (int i = 0; i < parameterModifiers.Count; i++)
        {
            PawnParameterModifier modifier = parameterModifiers[i];
            int value = agent.parameterDict[modifier.ParameterName];
            ModifyParameterEffect effect = new(agent, modifier.ParameterName, value, value + modifier.deltaValue);
            ret.effects.Add(effect);
            parameterToReset.Remove(modifier.ParameterName);    //不会重置技能影响的参数
        }
        foreach (string parameterName in parameterToReset)
        {
            int value = agent.parameterDict[parameterName];
            ModifyParameterEffect effect = new(agent, parameterName, value, 0);
            ret.effects.Add(effect);
        }
        for (int i = 0; i < buffOnAgent.Count; i++)
        {
            BuffEffect buffEffect = agent.BuffManager.MockAdd(buffOnAgent[i].so, agent, buffOnAgent[i].probability);
            buffEffect.randomValue = Effect.NextInt();
            ret.effects.Add(buffEffect);
        }
    }

    /// <summary>
    /// 模拟技能花费的时间
    /// </summary>
    public int MockTime(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target)
    {
        return Mathf.RoundToInt((1f - agent.speedUpRate.CurrentValue) * MockPrimitiveTime(agent, igm, position, target));
    }
    /// <summary>
    /// 模拟技能花费的时间（不考虑加速率）
    /// </summary>
    protected virtual int MockPrimitiveTime(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target)
    {
        return actionTime;
    }

    public virtual AnimationProcess GenerateAnimation()
    {
        return null;
    }
}
