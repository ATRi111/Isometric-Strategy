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
    public int time;

    public override void RefreshProperty()
    {
        base.RefreshProperty();
        actionTime.Refresh();
        (GridObject as MovableGridObject).RefreshProperty();
    }

    protected override void BeforeBattle()
    {
        base.BeforeBattle();
        time = actionTime.CurrentValue;   //�볡AT
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.Register(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameManager.Unregister(this);
    }
}