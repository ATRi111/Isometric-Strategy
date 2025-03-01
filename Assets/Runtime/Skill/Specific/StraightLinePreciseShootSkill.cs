using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "直线弱点射击", menuName = "技能/特殊/直线弱点射击")]
public class StraightLinePreciseShootSkill : StraightLineSkill
{
    public float damageAmplifier;
    public BuffSO so;

    protected override float MockDamageAmplifier(IsometricGridManager igm, PawnEntity agent, Entity victim, Vector3Int position, Vector3Int target)
    {
        if (victim is PawnEntity pawnVictim)
        {
            if (pawnVictim.BuffManager.FindEnabled(so) != null)
                return damageAmplifier;
        }
        return 0f;
    }

    public override void ExtractKeyWords(KeyWordList keyWordList)
    {
        base.ExtractKeyWords(keyWordList);
        keyWordList.Push(so.name, so.Description);
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        if(so != null)
        {
            sb.Append("目标处于");
            sb.Append(so.name);
            sb.Append("状态时,");
            sb.Append("伤害提高");
            sb.Append(damageAmplifier.ToString("P0"));
            sb.AppendLine();
        }
    }
}
