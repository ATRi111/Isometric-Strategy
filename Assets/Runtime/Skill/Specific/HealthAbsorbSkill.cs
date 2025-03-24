using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "������ȡ", menuName = "����/����/������ȡ")]
public class HealthAbsorbSkill : RangedSkill
{
    public float absorbPercent;

    protected override void MockOtherEffectOnAgent(IsometricGridManager igm, PawnEntity agent, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.MockOtherEffectOnAgent(igm, agent, position, target, ret);
        for (int i = 0; i < ret.effects.Count; i++)
        {
            if (ret.effects[i] is HPChangeEffect hpChangeEffect)
            {
                if (hpChangeEffect.probability == Effect.MaxProbability)
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
