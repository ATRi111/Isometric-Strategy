/// <summary>
/// Ĭ�ϵ�Ч�������Ķ���
/// </summary>
public class EffectAnimationProcess : AnimationProcess
{
    public Effect effect;

    public EffectAnimationProcess(Effect effect)
        : base()
    {
        this.effect = effect;
    }

    public override float MockLatency(IAnimationSource source)
    {
        return 0f;
    }

    public override void Play()
    {
        Complete(); //Ĭ�ϵ�Ч���������ź�������������Ӧ�̳д���䣩
    }

    public override void Apply()
    {
        effect?.Apply();
    }
}
