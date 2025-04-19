using Services;
using System;

[Serializable]
public abstract class BuffEffect : Effect
{
    protected static GameManager gameManager;

    static BuffEffect()
    {
        gameManager = ServiceLocator.Get<GameManager>();
    }

    protected override AnimationProcess GenerateAnimation_Local()
    {
        return new ObjectAnimationProcess(this,
            "结果特效_状态改变",
            victim.transform,
            victim.transform.position);
    }

    public Buff buff;
    protected BuffManager buffManager;
    protected BuffEffect(Entity victim, Buff buff, BuffManager buffManager, int probability = MaxProbability) 
        : base(victim, probability)
    {
        this.buff = buff;
        this.buffManager = buffManager;
    }
}
