using Character;
using System.Collections.Generic;

/// <summary>
/// 可行动的Entity
/// </summary>
public class PawnEntity : Entity
{
    [AutoComponent]
    public PawnBrain Brain { get; private set; }
    [AutoComponent]
    public MovableGridObject MovableGridObject { get; private set; }

    public EFaction faction;
    public PawnClass pClass;
    public PawnRace pRace;
    public CharacterProperty actionTime;
    /// <summary>
    /// 全局计时器的值达到此值时，轮到此角色行动
    /// </summary>
    public int time;

    public override void RefreshProperty()
    {
        base.RefreshProperty();
        actionTime.Refresh();
        MovableGridObject.RefreshProperty();
    }

    protected override void BeforeBattle()
    {
        base.BeforeBattle();
        time = actionTime.IntValue;   //入场AT
    }

    protected override void Awake()
    {
        base.Awake();
        pClass.Bind(this);
        pRace.Bind(this);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.Register(this);
        pClass.Register();
        pRace.Register();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameManager.Unregister(this);
        pClass.Unregister();
        pRace.Unregister();
    }
    /// <summary>
    /// 与目标为友方返回1，与目标为敌方返回-1，目标为中立或非Pawn返回0
    /// </summary>
    /// <returns></returns>
    public int CheckFaction(Entity entity)
    {
        PawnEntity pawn = entity as PawnEntity;
        if (pawn == null)
            return 0;
        return ((int)faction - 1) * ((int)pawn.faction - 1);
    }
}

public class Comparer_PawnEntity_ActionTime : IComparer<PawnEntity>
{
    public int Compare(PawnEntity x, PawnEntity y)
    {
        return x.actionTime.IntValue - y.actionTime.IntValue;
    }
}