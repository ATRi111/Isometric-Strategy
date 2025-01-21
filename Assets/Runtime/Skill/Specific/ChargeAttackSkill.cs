using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "��湥��", menuName = "����/����/��湥��")]
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
        sb.Append("ÿ���������ߴ˼���");
        sb.Append(powerAmplifier.ToString("P0"));
        sb.AppendLine("�˺�");
    }
}
