using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "��湥��", menuName = "����/����/��湥��")]
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
        sb.Append("ÿ���������ߴ˼���");
        sb.Append(powerAmplifier.ToString("P0"));
        sb.AppendLine("�˺�");
    }
}
