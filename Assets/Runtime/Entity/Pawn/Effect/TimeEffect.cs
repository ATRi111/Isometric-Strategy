public class TimeEffect : Effect
{
    public int prev, current;

    public TimeEffect(Entity target)
       : base(target)
    {   
        prev = current = (target as PawnEntity).time;
    }

    public TimeEffect(Entity target, int prev, int current)
        : base(target)
    {
        this.prev = prev;
        this.current = current;
    }

    public override bool Appliable => Pawnvictim.time == prev;

    public override bool Revokable => Pawnvictim.time == current;

    public override AnimationProcess GenerateAnimation()
    {
        return null; //TODO
    }

    public override void Apply()
    {
        base.Apply();
        Pawnvictim.time = current;
    }

    public override void Revoke()
    {
        base.Revoke();
        Pawnvictim.time = prev;
    }

    public override float PrimitiveValueFor(PawnEntity pawn)
    {
        throw new System.InvalidOperationException();
    }
}
