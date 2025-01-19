using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public string displayName;
    public string extraDescription;
    public int actionTime;
    public List<ParameterPreCondition> preConditions;
    public List<BuffPreCondition> buffPreConditions;
    public List<PawnParameterModifier> parameterOnAgent;
    public List<BuffModifier> buffOnAgent;

    /// <summary>
    /// �жϵ�ǰ�ܷ�ʹ�ô˼���
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
        return true;
    }

    /// <summary>
    /// ��ȡ��ѡ�ļ���ʩ��λ��
    /// </summary>
    public virtual void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        ret.Clear();
    }

    /// <summary>
    /// ģ�⼼�ܲ�����Ӱ��
    /// </summary>
    public virtual void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        ret.timeEffect.current += MockTime(agent, igm, position, target);
        HashSet<string> parameterToReset = PawnEntity.ParameterTable.resetParameters.ToHashSet();
        for (int i = 0; i < parameterOnAgent.Count; i++)
        {
            PawnParameterModifier modifier = parameterOnAgent[i];
            int value = agent.parameterDict[modifier.ParameterName];
            ModifyParameterEffect effect = new(agent, modifier.ParameterName, value, value + modifier.deltaValue);
            ret.effects.Add(effect);
            parameterToReset.Remove(modifier.ParameterName);    //�������ü���Ӱ��Ĳ���
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
    /// ģ�⼼�ܻ��ѵ�ʱ��
    /// </summary>
    public int MockTime(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target)
    {
        return Mathf.RoundToInt((1f - agent.speedUpRate.CurrentValue) * MockPrimitiveTime(agent, igm, position, target));
    }
    /// <summary>
    /// ģ�⼼�ܻ��ѵ�ʱ�䣨�����Ǽ����ʣ�
    /// </summary>
    protected virtual int MockPrimitiveTime(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target)
    {
        return actionTime;
    }

    public virtual AnimationProcess GenerateAnimation()
    {
        return null;
    }

    #region ����
    public string Description
    {
        get
        {
            StringBuilder sb = new();
            Describe(sb);
            sb.Append(extraDescription);
            return sb.ToString();
        }
    }

    protected virtual void Describe(StringBuilder sb)
    {
        DescribeTime(sb);
        if(preConditions.Count > 0)
            DescribePreConditions(sb);
        if (buffOnAgent.Count > 0)
            DescribeBuffOnAgent(sb);
        if(parameterOnAgent.Count > 0)
            DescribeParameterOnAgent(sb);
    }

    protected virtual void DescribePreConditions(StringBuilder sb)
    {
        sb.AppendLine("ʩ��������");
        for (int i = 0; i < preConditions.Count; i++)
        {
            preConditions[i].Describe(sb);
        }
        sb.AppendLine();
    }

    protected virtual void DescribeTime(StringBuilder sb)
    {
        sb.Append("����ʱ�����ģ�");
        sb.Append(actionTime);
        sb.AppendLine();
    }

    protected virtual void DescribeBuffOnAgent(StringBuilder sb)
    {
        for (int i = 0; i < buffOnAgent.Count; i++)
        {
            buffOnAgent[i].Describe(sb, "����");
        }
    }

    protected virtual void DescribeParameterOnAgent(StringBuilder sb)
    {
        for (int i = 0; i < parameterOnAgent.Count; i++)
        {
            parameterOnAgent[i].Describe(sb, "����");
        }
    }

    #endregion
}
