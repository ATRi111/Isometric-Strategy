public class Effect_HPChange : Effect
{
    public int prev, current;

    public Effect_HPChange(PawnEntity victim, int prev, int current)
        : base(victim)
    {
        this.prev = prev;
        this.current = current;
    }

    public override bool Appliable => victim.HealthComponent.HP == prev;

    public override bool Revokable => victim.HealthComponent.HP == current;

    public override AnimationProcess GenerateAnimation()
    {
        return null;    //TODO
    }

    public override void Apply()
    {
        base.Apply();
        victim.HealthComponent.HP = current;
        if (current == 0)
            victim.Die();
    }

    public override void Revoke()
    {
        base.Revoke();
        victim.HealthComponent.HP = prev;
        if(current == 0)
            victim.Revive();
    }
}
