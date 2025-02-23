using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "͵Ϯ", menuName = "����/����/͵Ϯ")]
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
        sb.Append("�ӱ��󹥻�����ʱ,�˺����");
        sb.Append(damageAmplifier.ToString("P0"));
    }
}
