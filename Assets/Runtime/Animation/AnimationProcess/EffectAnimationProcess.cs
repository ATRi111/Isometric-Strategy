/// <summary>
/// 默认的效果附带的动画
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
        Complete(); //默认的效果动画播放后立即结束（不应继承此语句）
    }

    public override void Apply()
    {
        effect?.Apply();
    }
}
