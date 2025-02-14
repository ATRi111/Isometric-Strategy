/// <summary>
/// Ч�������Ķ���
/// </summary>
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
        Complete(); //Ĭ�ϵ�Ч���������ź�������������Ӧ�̳д���䣩
    }

    public override void Apply()
    {
        effect?.Apply();
    }

}
