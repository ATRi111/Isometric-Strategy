using System;
using static UnityEngine.Rendering.DebugUI;
using System.Text;

[Serializable]
public class HPChangeEffect : Effect
{
    public int prev, current;

    public HPChangeEffect(Entity victim, int prev, int current, int probability = MaxProbability)
        : base(victim, probability)
    {
        this.prev = prev;
        this.current = current;
    }

    public override bool Appliable => victim.DefenceComponent.HP == prev;

    public override bool Revokable => victim.DefenceComponent.HP == current;

    public override AnimationProcess GenerateAnimation()
    {
        return null;    //TODO
    }

    public override void Apply()
    {
        base.Apply();
        victim.DefenceComponent.HP = current;
    }

    public override void Revoke()
    {
        base.Revoke();
        victim.DefenceComponent.HP = prev;
    }

    public override float ValueFor(PawnEntity pawn)
    {
        return (current - prev) * pawn.FactionCheck(victim);
    }
    public override void Describe(StringBuilder sb, bool result)
    {
        base.Describe(sb, result);
        sb.Append("的生命从");
        sb.Append(prev);
        sb.Append("变为");
        sb.Append(current);
        sb.AppendLine();
    }
}
