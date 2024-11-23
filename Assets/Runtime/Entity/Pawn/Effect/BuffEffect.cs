public abstract class BuffEffect : Effect
{
    protected Buff buff;
    protected BuffManager buffManager;
    protected BuffEffect(Entity victim, Buff buff, BuffManager buffManager, int probability = 100) : base(victim, probability)
    {
        this.buff = buff;
        this.buffManager = buffManager;
    }

    public override AnimationProcess GenerateAnimation()
    {
        return null;
    }
}
