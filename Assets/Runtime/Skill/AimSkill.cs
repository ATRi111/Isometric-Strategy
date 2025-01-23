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
/// 有目标型技能
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
    /// 模拟技能影响范围
    /// </summary>
    public abstract void MockArea(IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Vector3Int> ret);

    /// <summary>
    /// 判断某个entity是否是技能允许的目标
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
    /// 获取指定位置上的技能目标
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
    /// 获取技能命中的Entity
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
    /// 模拟技能威力
    /// </summary>
    protected virtual void MockPower(PawnEntity agent,  List<SkillPower> ret)
    {
        ret.Clear();
        ret.AddRange(powers);
        agent.OffenceComponent.ModifyPower?.Invoke(ret);
    }

    /// <summary>
    /// 模拟技能对指定目标造成的伤害
    /// </summary>
    protected virtual void MockDamageOnVictim(PawnEntity agent, Entity victim, List<SkillPower> powers, EffectUnit ret)
    {
        int r = Effect.NextInt();
        //计算伤害
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
        //死亡判定
        if (hp == 0)
        {
            effect = new DisableEntityEffect(victim);
            ret.effects.Add(effect);
        }
    }

    /// <summary>
    /// 模拟技能对指定目标施加的状态
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
    /// 模拟技能对目标造成伤害和施加状态以外的影响
    /// </summary>
    protected virtual void MockOtherEffectOnVictim(PawnEntity agent, Entity victim, EffectUnit ret)
    {

    }

    public override void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.Mock(agent, igm, position, target, ret);
        List<Entity> victims = new();
        GetVictims(agent, igm, position, target, victims);
        //计算附加威力
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

    #region 描述
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
            sb.Append("治疗力：");
            powers[0].Describe(sb);
            sb.AppendLine();
            return;
        }

        sb.AppendLine("威力：");
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
            buffOnVictim[i].Describe(sb, "目标");
        }
        sb.AppendLine();
    }

    protected virtual void DescribeVictim(StringBuilder sb)
    {
        switch(victimType)
        {
            case EVictimType.Pawn:
                sb.Append("仅以自身/友方/敌方角色为目标");
                sb.AppendLine();
                break;
            case EVictimType.Ally:
                sb.Append("仅以自身/友方角色为目标");
                sb.AppendLine();
                break;
            case EVictimType.Enemy:
                sb.Append("仅以敌方角色为目标");
                sb.AppendLine();
                break;
            default:
                break;
        }
    }

    #endregion
}
