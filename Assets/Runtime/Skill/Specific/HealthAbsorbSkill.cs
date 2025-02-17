using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "生命吸取", menuName = "技能/特殊/生命吸取")]
public class HealthAbsorbSkill : RangedSkill
{
    public float absorbPercent;

    protected override void MockDamageOnVictim(IsometricGridManager igm, PawnEntity agent, Entity victim, Vector3Int position, Vector3Int target, List<SkillPower> powers, EffectUnit ret)
    {
        base.MockDamageOnVictim(igm, agent, victim, position, target, powers, ret);
        int n = ret.effects.Count;  //中途可能会插入新元素
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
                    break;  //只有第一段伤害能触发吸血
                }
            }
        }
    }

    protected override void DescribePower(StringBuilder sb)
    {
        base.DescribePower(sb);
        sb.Append("自身回复造成伤害");
        sb.Append(absorbPercent.ToString("P0"));
        sb.Append("的生命");
        sb.AppendLine();
    }
}
