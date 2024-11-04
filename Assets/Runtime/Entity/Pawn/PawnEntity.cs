using Character;

/// <summary>
/// 可行动的Entity
/// </summary>
public class PawnEntity : Entity
{
    [AutoComponent]
    public PawnBrain Brain { get; private set; }

    public EFaction faction;
    public int actionTime;
    /// <summary>
    /// 总计时器的值达到此值时，轮到此角色行动
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
        waitTime = actionTime;   //入场AT
    }
}