using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "������ȡ", menuName = "����/����/������ȡ")]
public class HealthAbsorbSkill : RangedSkill
{
    public float absorbPercent;

    protected override void MockDamageOnVictim(IsometricGridManager igm, PawnEntity agent, Entity victim, Vector3Int position, Vector3Int target, List<SkillPower> powers, EffectUnit ret)
    {
        base.MockDamageOnVictim(igm, agent, victim, position, target, powers, ret);
        int n = ret.effects.Count;  //��;���ܻ������Ԫ��
        for (int i = 0; i < n; i++)
        {
            if(ret.effects[i] is HPChangeEffect hpChangeEffect)
            {
                if(agent.FactionCheck(hpChangeEffect.victim) < 0 && hpChangeEffect.probability == Effect.MaxProbability)
                {
                    int hp = agent.DefenceComponent.HP + Mathf.RoundToInt(absorbPercent * (hpChangeEffect.prev - hpChangeEffect.current));
                    HPChangeEffect absorb = new(agent, agent.DefenceComponent.HP, hp, hpChangeEffect.probability)
                    {
                        randomValue = hpChangeEffect.randomValue
                    };
                    ret.effects.Add(absorb);
                    break;  //ֻ�е�һ���˺��ܴ�����Ѫ
                }
            }
        }
    }

    protected override void DescribePower(StringBuilder sb)
    {
        base.DescribePower(sb);
        sb.Append("����ظ�����˺�");
        sb.Append(absorbPercent.ToString("P0"));
        sb.Append("������");
        sb.AppendLine();
    }
}
