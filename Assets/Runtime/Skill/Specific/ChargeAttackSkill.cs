using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "��湥��", menuName = "����/����/��湥��")]
public class ChargeAttackSkill : RangedSkill
{
    public float damageAmplifier;

    protected override float MockDamageAmplifier(IsometricGridManager igm, PawnEntity agent, Entity victim, Vector3Int position, Vector3Int target)
    {
        return agent.parameterDict[ChargeSkill.ChargeLevel] * damageAmplifier;
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.Append("ÿ���������ߴ˼���");
        sb.Append(damageAmplifier.ToString("P0"));
        sb.Append("�˺�");
        sb.AppendLine();
    }
}
