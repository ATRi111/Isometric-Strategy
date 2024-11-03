public class Effect_Time : Effect
{
    public int prev, current;

    public Effect_Time(PawnEntity target)
       : base(target)
    {
        prev = current = target.State.waitTime;
    }

    public Effect_Time(PawnEntity target, int prev, int current)
        : base(target)
    {
        this.prev = prev;
        this.current = current;
    }

    public override bool Appliable => victim.State.waitTime == prev;

    public override bool Revokable => victim.State.waitTime == current;

    public override AnimationProcess GenerateAnimation()
    {
        return null; //TODO
    }

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
