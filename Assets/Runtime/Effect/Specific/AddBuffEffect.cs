using System;
using System.Text;

[Serializable]
public class AddBuffEffect : BuffEffect
{
    public override bool Appliable => !buffManager.Contains(buff);

    public AddBuffEffect(Entity victim, Buff buff, BuffManager buffManager, int probability = MaxProbability) 
        : base(victim, buff, buffManager, probability)
    {
    }

    public override float ValueFor(PawnEntity pawn)
    {
        if (PawnVictim != null)
            return buff.so.ValueForVictim(PawnVictim) * pawn.Sensor.FactionCheck(victim);
        return 0f;
    }

    public override void Apply()
    {
        base.Apply();
        buffManager.Add(buff);
    }

    public override void Describe(StringBuilder sb, bool result)
    {
        base.Describe(sb, result);
        sb.Append("获得持续时间为");
        sb.Append(buff.endTime - gameManager.Time);
        sb.Append("的");
        sb.Append(buff.displayName);
        sb.Append("状态");
        sb.AppendLine();
    }
}
