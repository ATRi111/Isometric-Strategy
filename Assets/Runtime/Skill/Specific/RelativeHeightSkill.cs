using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "��Ը߶Ȳ�Ӱ���˺��ļ���", menuName = "����/����/��Ը߶Ȳ�Ӱ���˺��ļ���")]
public class RelativeHeightSkill : RangedSkill
{
    public bool higher;
    public float maxAmplifier;
    public float damageAmplifier;

    protected override float MockDamageAmplifier(IsometricGridManager igm, PawnEntity agent, Entity victim, Vector3Int position, Vector3Int target)
    {
        int h = agent.GridObject.CellPosition.z - victim.GridObject.CellPosition.z;
        if (!higher)
            h = -h;
        return Mathf.Clamp(h * damageAmplifier, 0, maxAmplifier);
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.Append("�����λ��ÿ��Ŀ��");
        if (higher)
            sb.Append("��");
        else
            sb.Append("��");
        sb.Append("һ��,");
        sb.AppendLine();
        sb.Append("�˺����");
        sb.Append(damageAmplifier.ToString("P0"));
        sb.Append("(���");
        sb.Append(maxAmplifier.ToString("P0"));
        sb.Append(")");
    }
}
