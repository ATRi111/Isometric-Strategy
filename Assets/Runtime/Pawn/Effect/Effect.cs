using System;

[Serializable]
public abstract class Effect
{
    public PawnEntity target;

    public Effect(PawnEntity target)
    {
        this.target = target;
    }

    public abstract bool Appliable { get; }
    public abstract bool Revokable { get; }

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
}
