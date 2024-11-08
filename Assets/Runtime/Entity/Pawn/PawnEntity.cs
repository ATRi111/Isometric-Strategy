using Character;

/// <summary>
/// 可行动的Entity
/// </summary>
public class PawnEntity : Entity
{
    [AutoComponent]
    public PawnBrain Brain { get; private set; }

    public EFaction faction;
    public IntProperty actionTime;
    /// <summary>
    /// 全局计时器的值达到此值时，轮到此角色行动
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
        time = actionTime.CurrentValue;   //入场AT
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