/// <summary>
/// ��ɫʩ�ż��ܸ����Ķ���
/// </summary>
public abstract class ActionAnimationProcess : AnimationProcess
{
    public PawnAction action;
    public ActionAnimationProcess(PawnAction action)
        : base()
    {
        this.action = action;
    }

    public override void Apply()
    {
        throw new System.NotImplementedException();
    }

    public override void Play()
    {
        throw new System.NotImplementedException();
    }
}
