using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "定身", menuName = "状态/定身")]
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
        sb.Append("被施加此状态时,等待时间增加");
        sb.Append(time);
        sb.AppendLine();
    }
}
