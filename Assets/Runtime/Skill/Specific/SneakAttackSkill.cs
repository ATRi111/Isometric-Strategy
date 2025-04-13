using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "戳刺", menuName = "技能/特殊/戳刺")]
public class SneakAttackSkill : RangedSkill
{
    public static bool SneakCheck(PawnEntity agent, Entity victim)
    {
        if (victim is PawnEntity pawnVictim)
        {
            Vector2Int direction = (Vector2Int)(victim.GridObject.CellPosition - agent.GridObject.CellPosition);
            return direction == pawnVictim.faceDirection;
        }
        return false;
    }

    public float damageAmplifier;
    public int ignorePierceResistance;

    protected override float MockDamageAmplifier(IsometricGridManager igm, PawnEntity agent, Entity victim, Vector3Int position, Vector3Int target)
    {
        float k = 1f;
        if (SneakCheck(agent, victim))
            k += damageAmplifier;
        int pierceResistance = victim.DefenceComponent.resistance[EDamageType.Pierce].IntValue;
        k *= pierceResistance / (pierceResistance - ignorePierceResistance);
        return k - 1f;
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.Append("无视目标");
        sb.Append(ignorePierceResistance);
        sb.Append("点穿刺抗性");
        sb.AppendLine();
        sb.Append("从背后攻击敌人时,伤害提高");
        sb.Append(damageAmplifier.ToString("P0"));
        sb.AppendLine();
    }
}
