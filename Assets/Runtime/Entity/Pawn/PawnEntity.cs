using Character;

/// <summary>
/// ���ж���Entity
/// </summary>
public class PawnEntity : Entity
{
    [AutoComponent]
    public PawnBrain Brain { get; private set; }

    public EFaction faction;
    public int actionTime;
    /// <summary>
    /// �ܼ�ʱ����ֵ�ﵽ��ֵʱ���ֵ��˽�ɫ�ж�
    /// </summary>
    public int waitTime;


    protected override void Register()
    {
        base.Register();
        GameManager.Register(this);
    }

    protected override void UnRegister()
    {
        base.UnRegister();
        GameManager.Unregister(this);
    }

    protected override void OnStartBattle()
    {
        base.OnStartBattle();
        waitTime = actionTime;   //�볡AT
    }
}