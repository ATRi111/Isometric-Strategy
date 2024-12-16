using Character;
using MyTool;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum EEquipmentSlot
{
    Weapon,
}

public class EquipmentManager : CharacterComponentBase
{
    [SerializeField]
    private SerializedDictionary<EEquipmentSlot, Equipment> equipmentDict;
    private PawnEntity pawn;

    public void Initialize()
    {
        equipmentDict.Refresh();
        foreach (Equipment equipment in equipmentDict.Values)
        {
            equipment.Register(pawn);
        }
    }

    public Equipment Get(EEquipmentSlot slot)
    {
        equipmentDict.TryGetValue(slot, out Equipment ret);
        return ret;
    }

    public void Equip(Equipment equipment)
    {
        if(equipmentDict.ContainsKey(equipment.slot))
        {
            Unequip(equipment.slot);
        }
        equipment.Register(pawn);
        equipmentDict.Add(equipment.slot, equipment);
    }

    public void Unequip(EEquipmentSlot slot)
    {
        if(equipmentDict.ContainsKey(slot))
        {
            equipmentDict[slot].Unregister(pawn);
            equipmentDict.Remove(slot);
        }
    }

    public void GetAll(List<Equipment> ret)
    {
        foreach (Equipment equipment in equipmentDict.Values)
        {
            ret.Add(equipment);
        }
    }

    public void UnEquipAll()
    {
        Array slots = Enum.GetValues(typeof(EEquipmentSlot));
        for (int i = 0; i < slots.Length; i++)
        {
            EEquipmentSlot slot = (EEquipmentSlot)slots.GetValue(i);
            Unequip(slot);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        pawn = entity as PawnEntity;
    }
}
