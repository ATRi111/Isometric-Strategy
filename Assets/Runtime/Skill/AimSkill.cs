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
            EVictimType.Ally => agent.Sensor.FactionCheck(victim) > 0,
            EVictimType.Enemy => agent.Sensor.FactionCheck(victim) < 0,
            _ => false,
        };
    }

    /// <summary>
    /// 获取指定位置上的技能目标
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
    /// 模拟技能对指定目标造成的伤害(含击退效果)
    /// </summary>
    protected void MockDamageOnVictim(IsometricGridManager igm, PawnEntity agent, Entity victim, Vector3Int position, Vector3Int target, List<SkillPower> powers, EffectUnit ret)
    {
        //计算伤害
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
    /// 模拟技能对指定目标的额外伤害倍率
    /// </summary>
    protected virtual float MockDamageAmplifier(IsometricGridManager igm, PawnEntity agent, Entity victim, Vector3Int position, Vector3Int target)
    {
        return 0f;
    }

    /// <summary>
    /// 模拟技能对指定目标施加的状态
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
    /// 模拟技能对目标的其他影响
    /// </summary>
    protected virtual void MockOtherEffectOnVictim(PawnEntity agent, Entity victim, EffectUnit ret)
    {

    }

    /// <summary>
    /// 模拟技能对自身造成的其他影响
    /// </summary>
    protected virtual void MockOtherEffectOnAgent(IsometricGridManager igm, PawnEntity agent, Vector3Int position, Vector3Int target,EffectUnit ret)
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
            MockDamageOnVictim(igm, agent, victims[i], position, target, tempPower, ret);
            if (victims[i] is PawnEntity pawn)
                MockBuffOnVictim(agent, pawn, ret);
            MockOtherEffectOnVictim(agent,victims[i], ret);
        }
        MockOtherEffectOnAgent(igm, agent, position, target, ret);
    }

    #region 描述

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

    public virtual void DescribeHitback(StringBuilder sb)
    {
        if (hitBackProbability < Effect.MaxProbability)
        {
            sb.Append("有");
            sb.Append(hitBackProbability);
            sb.Append("%的概率");
        }
        sb.Append("将目标击退一格");
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
                sb.Append("仅对角色生效");
                sb.AppendLine();
                break;
            case EVictimType.Ally:
                sb.Append("仅对自身/友方角色生效");
                sb.AppendLine();
                break;
            case EVictimType.Enemy:
                sb.Append("仅对敌方角色生效");
                sb.AppendLine();
                break;
            default:
                break;
        }
    }

    #endregion
}
