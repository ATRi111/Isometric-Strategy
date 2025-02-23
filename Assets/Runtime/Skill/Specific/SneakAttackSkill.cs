using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "偷袭", menuName = "技能/特殊/偷袭")]
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

    protected override float MockDamageAmplifier(IsometricGridManager igm, PawnEntity agent, Entity victim, Vector3Int position, Vector3Int target)
    {
        if (SneakCheck(agent, victim))
            return damageAmplifier;
        return 0f;
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.Append("从背后攻击敌人时,伤害提高");
        sb.Append(damageAmplifier.ToString("P0"));
    }
}
