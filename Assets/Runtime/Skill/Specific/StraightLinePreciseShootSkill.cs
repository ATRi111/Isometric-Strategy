using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "ֱ���������", menuName = "����/����/ֱ���������")]
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
            sb.Append("Ŀ�괦��");
            sb.Append(so.name);
            sb.Append("״̬ʱ,");
            sb.Append("�˺����");
            sb.Append(damageAmplifier.ToString("P0"));
            sb.AppendLine();
        }
    }
}
