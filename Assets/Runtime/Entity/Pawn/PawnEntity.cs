using Character;

/// <summary>
/// ���ж���Entity
/// </summary>
public class PawnEntity : Entity
{
    [AutoComponent]
    public PawnBrain Brain { get; private set; }

    public EFaction faction;
    public IntProperty actionTime;
    /// <summary>
    /// ȫ�ּ�ʱ����ֵ�ﵽ��ֵʱ���ֵ��˽�ɫ�ж�
    /// </summary>
    public int waitTime;

    public override void RefreshProperty()
    {
        base.RefreshProperty();
        actionTime.Refresh();
        (GridObject as MovableGridObject).RefreshProperty();
    }

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
        waitTime = actionTime.CurrentValue;   //�볡AT
    }
}