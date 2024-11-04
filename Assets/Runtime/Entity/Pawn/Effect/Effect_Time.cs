public class Effect_Time : Effect
{
    public int prev, current;

    public Effect_Time(Entity target)
       : base(target)
    {   
        prev = current = (target as PawnEntity).waitTime;
    }

    public Effect_Time(Entity target, int prev, int current)
        : base(target)
    {
        this.prev = prev;
        this.current = current;
    }

    public override bool Appliable => Pawnvictim.waitTime == prev;

    public override bool Revokable => Pawnvictim.waitTime == current;

    public override AnimationProcess GenerateAnimation()
    {
        return null; //TODO
    }

    public override void Apply()
    {
        base.Apply();
        Pawnvictim.waitTime = current;
    }

    public override void Revoke()
    {
        base.Revoke();
        Pawnvictim.waitTime = prev;
    }
}
