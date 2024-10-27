public class HPChangeEffect : Effect
{
    public int prev, current;

    public HPChangeEffect(PawnEntity target, int prev, int current)
        : base(target)
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
    }

    public override void Revoke()
    {
        base.Revoke();
        victim.State.HP = prev;
    }
}
