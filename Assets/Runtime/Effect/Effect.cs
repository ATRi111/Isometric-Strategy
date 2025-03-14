using MyTool;
using System;
using System.Text;

[Serializable]
public abstract class Effect : IAnimationSource
{
    public static IsometricGridManager Igm => IsometricGridManager.Instance;

    public const int MaxProbability = 100;

    public static readonly RandomGroup randomGroup;
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
    public bool NeverHappen => probability == 0;
    public bool WillHappen
    {
        get
        {
            if(AlwaysHappen)
                return true;
            if (NeverHappen)
                return false;
            if (randomValue == -1)
                randomValue = NextInt();
            return randomValue <= probability;
        }
    }

    /// <summary>
    /// �Ƿ�����(������)��Ч��
    /// </summary>
    public bool hidden;

    public Effect(Entity victim, int probability = MaxProbability)
    {
        this.victim = victim;
        this.probability = probability;
        randomValue = -1;
        hidden = false;
    }

    public abstract bool Appliable { get; }
    public abstract bool Revokable { get; }

    protected Effect joinedEffect;  //�ȴ���Effect�Ķ���������Ϻ�,�����Effect�ſ��Բ���
    protected AnimationProcess animation;

    public AnimationProcess GenerateAnimation()
    {
        animation ??= GenerateAnimation_Local();
        if (joinedEffect != null)
            animation.joinedAnimation = joinedEffect.GenerateAnimation();
        return animation;
    }

    protected virtual AnimationProcess GenerateAnimation_Local()
    {
        return new EffectAnimationProcess(this);
    }

    /// <summary>
    /// ʹ�������Ķ����ȵ�effect�����Ķ���������Ϻ󲥷�
    /// </summary>
    public void Join(Effect effect)
    {
        joinedEffect = effect;
    }

    public virtual void Play(AnimationManager animationManager, float latency)
    {
        AnimationProcess animation = GenerateAnimation();
        animationManager.Register(animation, latency);
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
    /// �����Ч����ĳ����ɫ�ļ�ֵ�������Ǹ��ʣ�
    /// </summary>
    public abstract float ValueFor(PawnEntity pawn);

    /// <param name="result">����ʱ�Ƿ���������Ϣ</param>
    public virtual void Describe(StringBuilder sb, bool result)
    {
        if (hidden)
            return;
        if (!result && !AlwaysHappen)
        {
            sb.Append(probability);
            sb.Append("%");
        }
        sb.Append("ʹ");
        sb.Append(victim.EntityNameWithColor);
    }
}
