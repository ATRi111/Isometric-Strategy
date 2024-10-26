public class HPChangeEffect : Effect
{
    public int prev, current;

    public HPChangeEffect(PawnEntity target, int prev, int current)
        : base(target)
    {
        this.prev = prev;
        this.current = current;
    }

    public override bool Appliable => victim.State.hp == prev;

    public override bool Revokable => victim.State.hp == current;

    public override void Apply()
    {
        base.Apply();
        victim.State.hp = current;
    }

    public override void Revoke()
    {
        base.Revoke();
        victim.State.hp = prev;
    }
}
