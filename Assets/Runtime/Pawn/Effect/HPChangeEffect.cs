public class HPChangeEffect : Effect
{
    public int prev, current;

    public HPChangeEffect(PawnEntity target, int prev, int current)
        : base(target)
    {
        this.prev = prev;
        this.current = current;
    }

    public override bool Appliable => target.State.HP == prev;

    public override bool Revokable => target.State.HP == current;

    public override void Apply()
    {
        base.Apply();
        target.State.HP = current;
    }

    public override void Revoke()
    {
        base.Revoke();
        target.State.HP = prev;
    }
}
