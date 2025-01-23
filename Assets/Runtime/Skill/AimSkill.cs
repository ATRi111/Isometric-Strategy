using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum EVictimType
{
    Nothing = 0,
    Entity = 1,
    Pawn = 2,
    Ally = 3,
    Enemy = 4,
}

/// <summary>
/// ��Ŀ���ͼ���
/// </summary>
public abstract class AimSkill : Skill
{
    public List<SkillPower> powers;
    public List<BuffModifier> buffOnVictim;
    public EVictimType victimType = EVictimType.Entity;
    public int minLayer = -2, maxLayer = 2;

    public bool LayerCheck(Vector3Int position, Vector3Int target)
    {
        int delta = target.z - position.z;
        return delta >= minLayer && delta <= maxLayer;
    }

    /// <summary>
    /// ģ�⼼��Ӱ�췶Χ
    /// </summary>
    public abstract void MockArea(IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Vector3Int> ret);

    /// <summary>
    /// �ж�ĳ��entity�Ƿ��Ǽ��������Ŀ��
    /// </summary>
    public virtual bool FilterVictim(PawnEntity agent, Entity victim)
    {
        return victimType switch
        {
            EVictimType.Entity => true,
            EVictimType.Pawn => victim is PawnEntity,
            EVictimType.Ally => agent.FactionCheck(victim) > 0,
            EVictimType.Enemy => agent.FactionCheck(victim) < 0,
            _ => false,
        };
    }

    /// <summary>
    /// ��ȡָ��λ���ϵļ���Ŀ��
    /// </summary>
    public virtual Entity GetVictimAt(PawnEntity agent, IsometricGridManager igm, Vector3Int target)
    {
        if(igm.EntityDict.ContainsKey(target))
        {
            Entity entity = igm.EntityDict[target];
            if (FilterVictim(agent, entity))
                return entity;
        }
        return null;
    }

    /// <summary>
    /// ��ȡ�������е�Entity
    /// </summary>
    public virtual void GetVictims(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Entity> ret)
    {
        ret.Clear();
        List<Vector3Int> area = new();
        MockArea(igm, position, target, area);
        for (int i = 0; i < area.Count; i++)
        {
            Entity entity = GetVictimAt(agent, igm, area[i]);
            if (entity != null)
                ret.Add(entity);
        }
    }

    /// <summary>
    /// ģ�⼼������
    /// </summary>
    protected virtual void MockPower(PawnEntity agent,  List<SkillPower> ret)
    {
        ret.Clear();
        ret.AddRange(powers);
        agent.OffenceComponent.ModifyPower?.Invoke(ret);
    }

    /// <summary>
    /// ģ�⼼�ܶ�ָ��Ŀ����ɵ��˺�
    /// </summary>
    protected virtual void MockDamageOnVictim(PawnEntity agent, Entity victim, List<SkillPower> powers, EffectUnit ret)
    {
        int r = Effect.NextInt();
        //�����˺�
        int damage = 0;
        DefenceComponent def = victim.DefenceComponent;
        for (int j = 0; j < powers.Count; j++)
        {
            float attackPower = agent.OffenceComponent.MockAttackPower(powers[j]);
            damage += def.MockDamage(powers[j].type, attackPower);
        }
        int hp = Mathf.Clamp(def.HP - damage, 0, def.maxHP.IntValue);
        Effect effect = new HPChangeEffect(victim, def.HP, hp);
        ret.effects.Add(effect);
        //�����ж�
        if (hp == 0)
        {
            effect = new DisableEntityEffect(victim);
            ret.effects.Add(effect);
        }
    }

    /// <summary>
    /// ģ�⼼�ܶ�ָ��Ŀ��ʩ�ӵ�״̬
    /// </summary>
    protected virtual void MockBuffOnVictim(PawnEntity agent, PawnEntity victim, EffectUnit ret)
    {
        for (int j = 0; j < buffOnVictim.Count; j++)
        {
            BuffEffect buffEffect = agent.BuffManager.MockAdd(buffOnVictim[j].so, victim, buffOnVictim[j].probability);
            buffEffect.randomValue = Effect.NextInt();
            ret.effects.Add(buffEffect);
        }
    }

    /// <summary>
    /// ģ�⼼�ܶ�Ŀ������˺���ʩ��״̬�����Ӱ��
    /// </summary>
    protected virtual void MockOtherEffectOnVictim(PawnEntity agent, Entity victim, EffectUnit ret)
    {

    }

    public override void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.Mock(agent, igm, position, target, ret);
        List<Entity> victims = new();
        GetVictims(agent, igm, position, target, victims);
        //���㸽������
        List<SkillPower> tempPower = new();
        MockPower(agent, tempPower);

        for (int i = 0; i < victims.Count; i++)
        {
            MockDamageOnVictim(agent, victims[i], powers, ret);
            if (victims[i] is PawnEntity pawn)
                MockBuffOnVictim(agent, pawn, ret);
            MockOtherEffectOnVictim(agent,victims[i], ret);
        }
    }

    #region ����
    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        if(powers.Count > 0)
            DescribePower(sb);
        if(buffOnVictim.Count > 0)
            DescribeBuffOnVictim(sb);
        DescribeVictim(sb);
    }

    protected virtual void DescribePower(StringBuilder sb)
    {
        if(powers.Count == 1 && powers[0].power < 0)
        {
            sb.Append("��������");
            powers[0].Describe(sb);
            sb.AppendLine();
            return;
        }

        sb.AppendLine("������");
        for (int i = 0; i < powers.Count; i++)
        {
            powers[i].Describe(sb);
        }
        sb.AppendLine();
    }

    protected virtual void DescribeBuffOnVictim(StringBuilder sb)
    {
        for (int i = 0; i < buffOnVictim.Count; i++)
        {
            buffOnVictim[i].Describe(sb, "Ŀ��");
        }
        sb.AppendLine();
    }

    protected virtual void DescribeVictim(StringBuilder sb)
    {
        switch(victimType)
        {
            case EVictimType.Pawn:
                sb.Append("��������/�ѷ�/�з���ɫΪĿ��");
                sb.AppendLine();
                break;
            case EVictimType.Ally:
                sb.Append("��������/�ѷ���ɫΪĿ��");
                sb.AppendLine();
                break;
            case EVictimType.Enemy:
                sb.Append("���Եз���ɫΪĿ��");
                sb.AppendLine();
                break;
            default:
                break;
        }
    }

    #endregion
}
