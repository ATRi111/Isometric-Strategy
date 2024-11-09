using MyTool;
using Services;
using System;

[Serializable]
public abstract class Effect
{
    protected static GameManager gameManager;

    static Effect()
    {
        gameManager = ServiceLocator.Get<GameManager>();
    }

    public const int MaxProbability = 100;

    public Entity victim;
    public PawnEntity Pawnvictim => victim as PawnEntity;

    public int probability;
    public int randomValue;
    public bool AlwaysHappen => probability == MaxProbability;
    public bool WillHappen
    {
        get
        {
            if(AlwaysHappen)
                return true;
            if (randomValue == -1)
                randomValue = RandomTool.GetGroup(ERandomGrounp.Battle).NextInt(0, MaxProbability);
            return randomValue <
                probability;
        }
    }

    public Effect(Entity victim, int probability = MaxProbability)
    {
        this.victim = victim;
        this.probability = probability;
        randomValue = -1;
    }

    public abstract bool Appliable { get; }
    public abstract bool Revokable { get; }

    public abstract AnimationProcess GenerateAnimation();

    public virtual void Play()
    {
        AnimationProcess animation = GenerateAnimation();
        if (animation != null)
            ServiceLocator.Get<AnimationManager>().Register(animation);
        else
            Apply();
    }
    public virtual void Apply()
    {
        if (!Appliable)
            throw new InvalidOperationException();
    }

    public virtual void Revoke()
    {
        if (!Revokable)
            throw new InvalidOperationException();
    }

    public override string ToString()
    {
        return GetType().Name;
    }
}
