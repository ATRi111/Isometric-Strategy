using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public string displayName;
    public int actionTime;
    public List<SkillPreCondition> preConditions;
    public List<PawnParameterModifier> parameterModifiers;

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
        for (int i = 0; i < parameterModifiers.Count; i++)
        {
            PawnParameterModifier modifier = parameterModifiers[i];
            int value = agent.parameterDict[modifier.parameterName];
            int r = Effect.NextInt();
            ModifyParameterEffect effect = new(agent, modifier.parameterName, value, value + modifier.deltaValue, modifier.probability)
            {
                randomValue = r
            };
            ret.effects.Add(effect);
        }
    }

    /// <summary>
    /// 模拟技能花费的时间
    /// </summary>
    public virtual int MockTime(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target)
    {
        return actionTime;
    }

    public virtual AnimationProcess GenerateAnimation()
    {
        return null;
    }
}
