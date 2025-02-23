using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "����", menuName = "״̬/����")]
public class HoldBuff : BuffSO
{
    public int time;

    public override void Register(PawnEntity pawn)
    {
        base.Register(pawn);
        pawn.time += time;
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.Append("��ʩ�Ӵ�״̬ʱ,�ȴ�ʱ������");
        sb.Append(time);
        sb.AppendLine();
    }
}
