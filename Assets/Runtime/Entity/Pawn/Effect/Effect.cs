using Services;
using System;

[Serializable]
public abstract class Effect
{
    public Entity victim;
    public PawnEntity Pawnvictim => victim as PawnEntity;

    public Effect(Entity victim)
    {
        this.victim = victim;
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
