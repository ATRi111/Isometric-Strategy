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
            "�����Ч_״̬�ı�",
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
