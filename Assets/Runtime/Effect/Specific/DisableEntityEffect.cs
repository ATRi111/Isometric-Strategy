using System.Text;

public class DisableEntityEffect : Effect
{
    public override bool Appliable => victim.gameObject.activeInHierarchy;

    public DisableEntityEffect(Entity victim, int probability = MaxProbability) : base(victim, probability)
    {
    }

    public override void Apply()
    {
        base.Apply();
        victim.Die();
    }

    public override float ValueFor(PawnEntity pawn)
    {
        return -100f * pawn.Sensor.FactionCheck(victim);
    }

    public override void Describe(StringBuilder sb, bool result)
    {
        base.Describe(sb, result);
        if (victim is PawnEntity)
            sb.Append("±»»÷µ¹");
        else
            sb.Append("±»ÆÆ»µ");
        sb.AppendLine();
    }
}
