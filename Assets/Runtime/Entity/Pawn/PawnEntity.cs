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
        waitTime = actionTime.CurrentValue;   //入场AT
    }
}