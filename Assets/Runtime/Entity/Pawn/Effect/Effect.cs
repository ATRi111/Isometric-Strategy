using MyTool;
using Services;
using System;

[Serializable]
public abstract class Effect
{
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
                randomValue = RandomTool.GetGroup(ERandomGrounp.Battle).NextInt(1, MaxProbability + 1);
            return randomValue <= probability;
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

    public virtual void Play(AnimationManager animationManager)
    {
        AnimationProcess animation = GenerateAnimation();
        if (animation != null)
            animationManager.Register(animation);
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

    /// <summary>
    /// 计算此效果对某个角色的价值（这里不考虑概率）
    /// </summary>
    public abstract float PrimitiveValueFor(PawnEntity pawn);

    public override string ToString()
    {
        return GetType().Name;
    }
}
