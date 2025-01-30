using Character;
using MyTool;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ж���Entity
/// </summary>
public class PawnEntity : Entity
{
    protected static ParameterTable parameterTable;
    public static ParameterTable ParameterTable
    {
        get
        {
            if (parameterTable == null)
            {
                parameterTable = Resources.Load<ParameterTable>(nameof(ParameterTable));
                parameterTable.Initialize();
            }
            return parameterTable;
        }
    }

    [AutoComponent]
    public PawnBrain Brain { get; private set; }
    [AutoComponent]
    public SkillManager SkillManager { get; private set; }
    [AutoComponent]
    public PawnSensor Sensor { get; private set; }

    [AutoComponent]
    public MovableGridObject MovableGridObject { get; private set; }
    [AutoComponent]
    public OffenceComponent OffenceComponent { get; private set; }

    [AutoComponent]
    public BuffManager BuffManager { get; protected set; }
    [AutoComponent]
    public EquipmentManager EquipmentManager { get; protected set; }

    public EFaction faction;
    public bool targetToKill;
    public PawnClass pClass;
    public Race race;
    public CharacterProperty actionTime;
    public CharacterProperty speedUpRate;

    /// <summary>
    /// ȫ�ּ�ʱ����ֵ�ﵽ��ֵʱ���ֵ��˽�ɫ�ж�
    /// </summary>
    public int time;

    public bool hidden;

    public CounterDictionary parameterDict;

    public List<string> GetVisibleParameters()
    {
        List<string> ret = new(); 
        foreach (KeyValuePair<string,int> pair in parameterDict.dict)
        {
            Parameter parameter = parameterTable[pair.Key];
            if (!parameter.hidden && pair.Value != 0)
                ret.Add(pair.Key);
        }
        return ret;
    }

    /// <summary>
    /// ��Ŀ��Ϊ�ѷ�����1����Ŀ��Ϊ�з�����-1��Ŀ��Ϊ�������Pawn����0
    /// </summary>
    /// <returns></returns>
    public int FactionCheck(Entity entity)
    {
        PawnEntity pawn = entity as PawnEntity;
        if (pawn == null)
            return 0;
        return ((int)faction - 1) * ((int)pawn.faction - 1);
    }

    protected override void OnTick(int time)
    {
        base.OnTick(time);
        BuffManager.OnTick(time);
    }

    public override void RefreshProperty()
    {
        base.RefreshProperty();
        actionTime.Refresh();
        speedUpRate.Refresh();
        MovableGridObject.RefreshProperty();
        OffenceComponent.RefreshProperty();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.Register(this);
        EquipmentManager.Initialize(this);
        pClass.Register(this);
        race.Register(this);
        RefreshProperty();
        time += actionTime.IntValue;   //�볡AT
        DefenceComponent.HP = DefenceComponent.maxHP.IntValue;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameManager.Unregister(this);
        pClass.Unregister(this);
        race.Unregister(this);
        BuffManager.Clear();
        parameterDict.Clear();
        time = 0;
    }
}

public class Comparer_PawnEntity_ActionTime : IComparer<PawnEntity>
{
    public int Compare(PawnEntity x, PawnEntity y)
    {
        return x.actionTime.IntValue - y.actionTime.IntValue;
    }
}