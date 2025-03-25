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

    public override void Apply()
    {
        base.Apply();
        PawnVictim.time = current;
    }

    public override float ValueFor(PawnEntity pawn)
    {
        return (prev - current) * pawn.Sensor.FactionCheck(victim);
    }

    public override void Describe(StringBuilder sb, bool result)
    {
        base.Describe(sb, result);
        sb.Append("�ĵȴ�ʱ��");
        if(current > prev)
        {
            sb.Append("����");
            sb.Append(current - prev);
        }
        else
        {
            sb.Append("����");
            sb.Append(prev - current);
        }
        sb.AppendLine();
    }
}
