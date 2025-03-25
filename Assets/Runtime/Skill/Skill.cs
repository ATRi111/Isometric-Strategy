using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class Skill : ScriptableObject , IDescription
{
    public string displayName;
    public Sprite icon;

    public string extraDescription;
    public int actionTime;
    public string animationName;
    public EPawnAnimationState pawnAnimationState;
    public float movementLatency;
    public bool weaponAnimation;

    public List<ParameterPreCondition> preConditions;
    public List<BuffPreCondition> buffPreConditions;
    public List<WeatherPreCondition> weatherPreConditions;
    public List<PawnParameterModifier> parameterOnAgent;
    public List<BuffModifier> buffOnAgent;

    public virtual bool Offensive => false;

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
        for (int i = 0; i < weatherPreConditions.Count; i++)
        {
            if (!weatherPreConditions[i].Verify(igm.BattleField))
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
        HashSet<string> parameterToReset = PawnEntity.ParameterTable.GetResetParameters();
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
            if (value != 0)
            {
                ModifyParameterEffect effect = new(agent, parameterName, value, 0)
                {
                    reset = true
                };
                ret.effects.Add(effect);   //������Ч������δӦ��Ч������˲���Ӱ��֮����˺������
            }
        }
        for (int i = 0; i < buffOnAgent.Count; i++)
        {
            BuffEffect buffEffect = agent.BuffManager.MockAdd(buffOnAgent[i].so, buffOnAgent[i].probability);
            buffEffect.randomValue = Effect.NextInt();
            ret.effects.Add(buffEffect);
        }
    }

    /// <summary>
    /// ģ�⼼�ܻ��ѵ�ʱ��
    /// </summary>
    public virtual int MockTime(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target)
    {
        return Mathf.RoundToInt((1f - agent.speedUpRate.CurrentValue) * MockPrimitiveTime(agent, igm, position, target));
    }
    /// <summary>
    /// ģ�⼼�ܻ��ѵ�ԭʼʱ��
    /// </summary>
    protected virtual int MockPrimitiveTime(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target)
    {
        return actionTime;
    }

    /// <summary>
    /// ģ�⼼�ܶ���������ǰ���㶯�������ʱ��
    /// </summary>
    public virtual AnimationProcess MockAnimation(PawnAction action)
    {
        if (!string.IsNullOrWhiteSpace(animationName))
        {
            ObjectAnimationProcess animationProcess = new(
                action, animationName,
                action.agent.transform,
                action.agent.transform.position);
            return animationProcess;
        }
        return null;
    }

    /// <summary>
    /// �������ﶯ��
    /// </summary>
    public virtual void PlayMovement(PawnAnimator pawnAnimator)
    {
        if (pawnAnimator != null && pawnAnimationState != EPawnAnimationState.Walk)
            pawnAnimator.Play(pawnAnimationState.ToString(), movementLatency, weaponAnimation);
    }

    #region ����

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
        if(preConditions.Count + buffPreConditions.Count + weatherPreConditions.Count > 0)
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
        for (int i = 0; i < buffPreConditions.Count; i++)
        {
            buffPreConditions[i].Describe(sb);
        }
        for (int i = 0; i < weatherPreConditions.Count; i++)
        {
            weatherPreConditions[i].Describe(sb);
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
