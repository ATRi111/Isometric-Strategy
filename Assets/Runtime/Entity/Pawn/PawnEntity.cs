using Character;
using System.Collections.Generic;

/// <summary>
/// ���ж���Entity
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
    /// ȫ�ּ�ʱ����ֵ�ﵽ��ֵʱ���ֵ��˽�ɫ�ж�
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
        time = actionTime.IntValue;   //�볡AT
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
    /// ��Ŀ��Ϊ�ѷ�����1����Ŀ��Ϊ�з�����-1��Ŀ��Ϊ�������Pawn����0
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