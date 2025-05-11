using Character;
using MyTool;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可行动的Entity
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
#if UNITY_EDITOR
            if (!Application.isPlaying)
                parameterTable.Initialize();
#endif
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
    [AutoComponent]
    public PawnAnimator PawnAnimator { get; protected set; }

    public Sprite icon;
    public Sprite tachie;
    public EFaction faction;
    public bool taskTarget;
    public PawnClass pClass;
    public Race race;
    public CharacterProperty actionTime;
    public CharacterProperty speedUpRate;
    public CharacterProperty hatredLevel;
    [NonSerialized]
    public Vector2Int faceDirection;
    [NonSerialized]
    public int sortingValue;    //多个角色等待时间相同，根据此值进行排序

    /// <summary>
    /// 全局计时器的值达到此值时，轮到此角色行动
    /// </summary>
    public int time;

    public bool hidden;

    public CounterDictionary parameterDict;

    public List<string> GetVisibleParameters()
    {
        List<string> ret = new();
        foreach (KeyValuePair<string, int> pair in parameterDict.dict)
        {
            Parameter parameter = parameterTable[pair.Key];
            if (!parameter.hidden)
                ret.Add(pair.Key);
        }
        return ret;
    }

    /// <summary>
    /// 与目标为友方返回1，与目标为敌方返回-1，目标为中立或非Pawn返回0
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

    protected override void BeforeBattle()
    {
        base.BeforeBattle();
        EquipmentManager.ApplyAllParameter();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.Register(this);
        EquipmentManager.Register(this);
        pClass.Register(this);
        race.Register(this);
        RefreshProperty();
        time += actionTime.IntValue;   //入场AT
        DefenceComponent.HP = DefenceComponent.maxHP.IntValue;
        faceDirection = Vector2Int.left;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameManager.Unregister(this);
        EquipmentManager.Unregister(this);
        pClass.Unregister(this);
        race.Unregister(this);
        BuffManager.Clear();
        parameterDict.Clear();
        time = 0;
    }

    public override string EntityNameWithColor
    {
        get
        {
            string color = faction switch
            {
                EFaction.Ally => FontUtility.SpringGreen3,
                EFaction.Enemy => FontUtility.Red,
                _ => FontUtility.Black
            };
            return EntityName.ColorText(color);
        }
    }
}

public class Comparer_PawnEntity_ActionTime : IComparer<PawnEntity>
{
    public int Compare(PawnEntity x, PawnEntity y)
    {
        if (x.actionTime != y.actionTime)
            return x.actionTime.IntValue - y.actionTime.IntValue;
        return x.sortingValue - y.sortingValue;
    }
}