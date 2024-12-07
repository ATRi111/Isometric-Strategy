using MyTool;
using System;

[Serializable]
public abstract class Effect
{
    public const int MaxProbability = 100;

    private static readonly RandomGroup randomGroup;
    static Effect()
    {
        randomGroup = RandomTool.GetGroup(ERandomGrounp.Battle);
    }
    
    public static int NextInt()
        => randomGroup.NextInt(1, MaxProbability + 1);

    public Entity victim;
    public PawnEntity PawnVictim => victim as PawnEntity;

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
                randomValue = NextInt();
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
    /// 计算此效果对某个角色的价值（不考虑概率）
    /// </summary>
    public abstract float PrimitiveValueFor(PawnEntity pawn);

    public override string ToString()
    {
        return GetType().Name;
    }
}
