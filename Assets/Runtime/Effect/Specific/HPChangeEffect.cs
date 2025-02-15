using System;
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

    protected override AnimationProcess GenerateAnimation_Local()
    {
        FollowHPBar hpBar = victim.GetComponentInChildren<FollowHPBar>();
        return new ObjectAnimationProcess(this,
            "DamageNumberUI",
            hpBar.transform,
            hpBar.UseDamageNumberPosition());
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
        sb.Append("��������");
        sb.Append(prev);
        sb.Append("��Ϊ");
        sb.Append(current);
        sb.AppendLine();
    }
}
