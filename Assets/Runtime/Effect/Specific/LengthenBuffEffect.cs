using MyTool;
using System;
using System.Text;

[Serializable]
public class LengthenBuffEffect : BuffEffect
{
    public int endTime_prev;
    public int endTime;
    public override bool Appliable => buffManager.Contains(buff) && buff.endTime == endTime_prev;

    public override bool Revokable => buffManager.Contains(buff) && buff.endTime == endTime;

    public LengthenBuffEffect(Entity victim, Buff buff, int endTime, BuffManager buffManager, int probability = MaxProbability) 
        : base(victim, buff, buffManager, probability)
    {
        endTime_prev = buff.endTime;
        this.endTime = endTime;
    }

    public override float PrimitiveValueFor(PawnEntity pawn)
    {
        int delta = endTime - buff.endTime;
        return delta / buff.so.duration * buff.so.primitiveValue * pawn.FactionCheck(victim);
    }

    public override void Apply()
    {
        base.Apply();
        buff.endTime = endTime;
    }

    public override void Revoke()
    {
        base.Revoke();
        buff.endTime = endTime_prev;
    }

    public override void Describe(StringBuilder sb, bool result)
    {
        base.Describe(sb, result);
        sb.Append("的");
        sb.Append(buff.displayName.Bold());
        sb.Append("状态的持续时间延长");
        sb.Append(endTime - endTime_prev);
        sb.AppendLine();
    }
}
