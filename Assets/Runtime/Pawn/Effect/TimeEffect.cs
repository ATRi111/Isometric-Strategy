public class TimeEffect : Effect
{
    public int prev, current;

    public TimeEffect(PawnEntity target, int prev, int current)
        : base(target)
    {
        this.prev = prev;
        this.current = current;
    }

    public override bool Appliable => target.State.WT == prev;

    public override bool Revokable => target.State.WT == current;

    public override void Apply()
    {
        base.Apply();
        target.State.WT = current;
    }

    public override void Revoke()
    {
        base.Revoke();
        target.State.WT = prev;
    }
}
