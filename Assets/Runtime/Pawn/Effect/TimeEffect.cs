public class TimeEffect : Effect
{
    public int prev, current;

    public TimeEffect(PawnEntity target)
       : base(target)
    {
        prev = current= target.State.waitTime;
    }

    public TimeEffect(PawnEntity target, int prev, int current)
        : base(target)
    {
        this.prev = prev;
        this.current = current;
    }

    public override bool Appliable => victim.State.waitTime == prev;

    public override bool Revokable => victim.State.waitTime == current;

    public override void Apply()
    {
        base.Apply();
        victim.State.waitTime = current;
    }

    public override void Revoke()
    {
        base.Revoke();
        victim.State.waitTime = prev;
    }
}
