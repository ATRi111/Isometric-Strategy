using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "冲锋攻击", menuName = "技能/特殊/冲锋攻击")]
public class ChargeAttackSkill : RangedSkill
{
    public float powerAmplifier;

    protected override void MockPower(PawnEntity agent, List<SkillPower> ret)
    {
        ret.Clear();
        for (int i = 0; i < powers.Count; i++)
        {
            SkillPower temp = powers[i];
            temp.power = Mathf.RoundToInt(ret[i].power * (1f + powerAmplifier * agent.parameterDict[ChargeSkill.ChargeLevel]));
            ret.Add(temp);
        }
        agent.OffenceComponent.ModifyPower?.Invoke(ret);
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.Append("每层冲锋层数提高此技能");
        sb.Append(powerAmplifier.ToString("P0"));
        sb.AppendLine("伤害");
    }
}
