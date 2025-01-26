using System;
using System.Text;

[Serializable]
public class TimeEffect : Effect
{
    public int prev, current;

    public TimeEffect(Entity target)
       : base(target)
    {   
        prev = current = (target as PawnEntity).time;
    }

    public TimeEffect(Entity target, int prev, int current)
        : base(target)
    {
        this.prev = prev;
        this.current = current;
    }

    public override bool Appliable => PawnVictim.time == prev;

    public override bool Revokable => PawnVictim.time == current;

    public override AnimationProcess GenerateAnimation()
    {
        return null; //TODO
    }

    public override void Apply()
    {
        base.Apply();
        PawnVictim.time = current;
    }

    public override void Revoke()
    {
        base.Revoke();
        PawnVictim.time = prev;
    }

    public override float ValueFor(PawnEntity pawn)
    {
        return (prev - current) * pawn.FactionCheck(victim);
    }

    public override void Describe(StringBuilder sb, bool result)
    {
        base.Describe(sb, result);
        sb.Append("的等待时间增加");
        sb.Append(current - prev);
        sb.AppendLine();
    }
}
