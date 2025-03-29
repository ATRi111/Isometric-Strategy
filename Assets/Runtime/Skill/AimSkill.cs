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
    public int hitBackProbability;
    public EVictimType victimType = EVictimType.Entity;
    public int minLayer = -2, maxLayer = 2;

    public override bool Offensive => powers.Count > 0 && powers[0].power > 0 
        || buffOnVictim.Count > 0 && buffOnVictim[0].so.removeFlag != ERemoveFlag.Buff;

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
            EVictimType.Ally => agent.Sensor.FactionCheck(victim) > 0,
            EVictimType.Enemy => agent.Sensor.FactionCheck(victim) < 0,
            _ => false,
        };
    }

    /// <summary>
    /// ��ȡָ��λ���ϵļ���Ŀ��
    /// </summary>
    public virtual Entity GetVictimAt(PawnEntity agent, IsometricGridManager igm, Vector3Int target)
    {
        if(igm.entityDict.ContainsKey(target))
        {
            Entity entity = igm.entityDict[target];
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
    /// ģ�⼼�ܶ�ָ��Ŀ����ɵ��˺�(������Ч��)
    /// </summary>
    protected void MockDamageOnVictim(IsometricGridManager igm, PawnEntity agent, Entity victim, Vector3Int position, Vector3Int target, List<SkillPower> powers, EffectUnit ret)
    {
        //�����˺�
        int damage = 0;
        DefenceComponent def = victim.DefenceComponent;
        BattleField battleField = igm.BattleField;
        for (int j = 0; j < powers.Count; j++)
        {
            float attackPower = agent.OffenceComponent.MockAttackPower(powers[j]);
            attackPower *= battleField.MockPowerMultiplier(powers[j].type);
            damage += def.MockDamage(powers[j].type, attackPower);
        }

        damage = Mathf.RoundToInt(damage * (1f + MockDamageAmplifier(igm, agent, victim, position, target)));
        
        int hp = Mathf.Clamp(def.HP - damage, 0, def.maxHP.IntValue);

        if (def.HP != hp)
        {
            HPChangeEffect hpChangeEffect = new(victim, def.HP, hp);
            ret.effects.Add(hpChangeEffect);
            if (hp == 0)
            {
                DisableEntityEffect disableEntityEffect = new(victim);
                disableEntityEffect.Join(hpChangeEffect);
                ret.effects.Add(disableEntityEffect);
            }
        }
        else if (hp > 0 && victim is PawnEntity pawnVictim && hitBackProbability > 0)
        {
            HitBackUtility.MockHitBack(igm, position, target, pawnVictim, hp, hitBackProbability, ret);
        }
    }

    /// <summary>
    /// ģ�⼼�ܶ�ָ��Ŀ��Ķ����˺�����
    /// </summary>
    protected virtual float MockDamageAmplifier(IsometricGridManager igm, PawnEntity agent, Entity victim, Vector3Int position, Vector3Int target)
    {
        return 0f;
    }

    /// <summary>
    /// ģ�⼼�ܶ�ָ��Ŀ��ʩ�ӵ�״̬
    /// </summary>
    protected virtual void MockBuffOnVictim(PawnEntity agent, PawnEntity victim, EffectUnit ret)
    {
        for (int i = 0; i < buffOnVictim.Count; i++)
        {
            BuffEffect buffEffect;
            if (!buffOnVictim[i].remove)
                buffEffect = victim.BuffManager.MockAdd(buffOnVictim[i].so, buffOnVictim[i].probability);
            else
                buffEffect = victim.BuffManager.MockRemove(buffOnVictim[i].so, buffOnVictim[i].probability);

            if (buffEffect != null)
            {
                if (!buffEffect.AlwaysHappen)
                    buffEffect.randomValue = Effect.NextInt();
                ret.effects.Add(buffEffect);
            }
        }
    }

    /// <summary>
    /// ģ�⼼�ܶ�Ŀ�������Ӱ��
    /// </summary>
    protected virtual void MockOtherEffectOnVictim(PawnEntity agent, Entity victim, EffectUnit ret)
    {

    }

    /// <summary>
    /// ģ�⼼�ܶ�������ɵ�����Ӱ��
    /// </summary>
    protected virtual void MockOtherEffectOnAgent(IsometricGridManager igm, PawnEntity agent, Vector3Int position, Vector3Int target,EffectUnit ret)
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
            MockDamageOnVictim(igm, agent, victims[i], position, target, tempPower, ret);
            if (victims[i] is PawnEntity pawn)
                MockBuffOnVictim(agent, pawn, ret);
            MockOtherEffectOnVictim(agent,victims[i], ret);
        }
        MockOtherEffectOnAgent(igm, agent, position, target, ret);
    }

    #region ����

    public override void ExtractKeyWords(KeyWordList keyWordList)
    {
        base.ExtractKeyWords(keyWordList);
        for (int i = 0; i < buffOnVictim.Count; i++)
        {
            BuffSO so = buffOnVictim[i].so;
            keyWordList.Push(so.name, so.Description);
        }
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        if(powers.Count > 0)
            DescribePower(sb);
        if(buffOnVictim.Count > 0)
            DescribeBuffOnVictim(sb);
        if (hitBackProbability > 0)
            DescribeHitback(sb);
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

    public virtual void DescribeHitback(StringBuilder sb)
    {
        if (hitBackProbability < Effect.MaxProbability)
        {
            sb.Append("��");
            sb.Append(hitBackProbability);
            sb.Append("%�ĸ���");
        }
        sb.Append("��Ŀ�����һ��");
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
                sb.Append("���Խ�ɫ��Ч");
                sb.AppendLine();
                break;
            case EVictimType.Ally:
                sb.Append("��������/�ѷ���ɫ��Ч");
                sb.AppendLine();
                break;
            case EVictimType.Enemy:
                sb.Append("���Եз���ɫ��Ч");
                sb.AppendLine();
                break;
            default:
                break;
        }
    }

    #endregion
}
