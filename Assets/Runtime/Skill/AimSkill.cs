using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// ��Ŀ���ͼ���
/// </summary>
public abstract class AimSkill : Skill
{
    public const int MaxAccuracy = 100;
    public int accuracy = MaxAccuracy;
    public List<SkillPower> powers;
    public List<BuffModifier> buffOnVictim;
    public int minLayer = -2, maxLayer = 2;

    public bool LayerCheck(Vector3Int position, Vector3Int target)
    {
        int delta = target.z - position.z;
        return delta >= minLayer && delta <= maxLayer;
    }

    /// <summary>
    /// ��ȡ�������е�Entity
    /// </summary>
    public virtual void GetVictims(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Entity> ret)
    {
        ret.Clear();
    }

    /// <summary>
    /// �ж�ĳ��entity�Ƿ��Ǽ��������Ŀ��
    /// </summary>
    public virtual bool FilterVictim(Entity entity)
    {
        return true;
    }

    public override void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.Mock(agent, igm, position, target, ret);
        List<Entity> victims = new();
        GetVictims(agent, igm, position, target, victims);
        for (int i = 0; i < victims.Count; i++)
        {
            int r = Effect.NextInt();
            //�˺�
            int damage = 0;
            DefenceComponent def = victims[i].DefenceComponent;
            for (int j = 0; j < powers.Count; j++)
            {
                float attackPower = agent.OffenceComponent.MockAttackPower(powers[j]);
                damage += def.MockDamage(powers[j].type, attackPower);
            }
            int hp = Mathf.Clamp(def.HP - damage, 0, def.maxHP.IntValue);
            Effect effect = new HPChangeEffect(victims[i], def.HP, hp, accuracy)
            {
                randomValue = r
            };
            ret.effects.Add(effect);
            //����
            if (hp == 0)
            {
                effect = new DisableEntityEffect(victims[i])
                {
                    randomValue = r
                };
                ret.effects.Add(effect);
            }
            //Buff
            PawnEntity pawn = victims[i] as PawnEntity;
            if(pawn != null)
            {
                for (int j = 0; j < buffOnVictim.Count; j++)
                {
                    BuffEffect buffEffect = agent.BuffManager.MockAdd(buffOnVictim[j].so, pawn, buffOnVictim[j].probability);
                    buffEffect.randomValue = Effect.NextInt();
                    ret.effects.Add(buffEffect);
                }
            }
        }
    }

    #region ����
    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        DescribeAccuracy(sb);
        if(powers.Count > 0)
            DescribePower(sb);
        if(buffOnVictim.Count > 0)
            DescribeBuffOnVictim(sb);
    }

    protected virtual void DescribeAccuracy(StringBuilder sb)
    {
        sb.Append("������:");
        sb.Append(accuracy);
        sb.Append("%");
        sb.AppendLine();
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
    }

    #endregion
}
