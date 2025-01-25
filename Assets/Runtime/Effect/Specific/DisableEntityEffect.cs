using System.Text;

public class DisableEntityEffect : Effect
{
    public override bool Appliable => victim.gameObject.activeInHierarchy;

    public override bool Revokable => !victim.gameObject.activeInHierarchy;

    public DisableEntityEffect(Entity victim, int probability = 100) : base(victim, probability)
    {
    }

    public override AnimationProcess GenerateAnimation()
    {
        //TODO
        return null;
    }

    public override void Apply()
    {
        base.Apply();
        victim.Die();
    }

    public override void Revoke()
    {
        base.Revoke();
        victim.Revive();
    }

    public override float PrimitiveValueFor(PawnEntity pawn)
    {
        return -100f * pawn.FactionCheck(victim);
    }

    public override void Describe(StringBuilder sb, bool result)
    {
        base.Describe(sb, result);
        sb.Append("±»»÷µ¹");
        sb.AppendLine();
    }
}
