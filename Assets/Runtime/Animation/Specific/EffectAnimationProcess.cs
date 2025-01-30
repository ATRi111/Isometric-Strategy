public class EffectAnimationProcess : AnimationProcess
{
    public Effect effect;

    public EffectAnimationProcess(Effect effect)
        : base()
    {
        this.effect = effect;
    }

    public override void Play()
    {
        Complete();     //�޶������ݣ����ż�����
    }

    public override void Apply()
    {
        effect?.Apply();
    }
}
