using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "冲锋攻击", menuName = "技能/特殊/冲锋攻击")]
public class ChargeAttackSkill : RangedSkill
{
    public float powerAmplifier;

    protected override void MockPower(PawnEntity agent, List<SkillPower> ret)
    {
        base.MockPower(agent, ret);
        for (int i = 0; i < ret.Count; i++)
        {
            ret[i].power = Mathf.RoundToInt(ret[i].power * (1f + powerAmplifier * agent.parameterDict[ChargeSkill.ParameterName]));
        }
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.Append("每层冲锋层数提高此技能");
        sb.Append(powerAmplifier.ToString("P0"));
        sb.AppendLine("伤害");
    }
}
