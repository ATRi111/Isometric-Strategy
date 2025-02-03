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
        Complete();     //无动画内容，播放即结束
    }

    public override void Apply()
    {
        effect?.Apply();
    }
}
