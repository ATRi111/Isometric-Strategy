public class Effect_HPChange : Effect
{
    public int prev, current;

    public Effect_HPChange(PawnEntity victim, int prev, int current)
        : base(victim)
    {
        this.prev = prev;
        this.current = current;
    }

    public override bool Appliable => victim.State.HP == prev;

    public override bool Revokable => victim.State.HP == current;

    public override void Apply()
    {
        base.Apply();
        victim.State.HP = current;
        if (current == 0)
            victim.Die();
    }

    public override void Revoke()
    {
        base.Revoke();
        victim.State.HP = prev;
        if(current == 0)
            victim.Revive();
    }
}
