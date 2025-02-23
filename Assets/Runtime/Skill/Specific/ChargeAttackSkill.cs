using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "冲锋攻击", menuName = "技能/特殊/冲锋攻击")]
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
        sb.Append("每层冲锋层数提高此技能");
        sb.Append(damageAmplifier.ToString("P0"));
        sb.Append("伤害");
        sb.AppendLine();
    }
}
