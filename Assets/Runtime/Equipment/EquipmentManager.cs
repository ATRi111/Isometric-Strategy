using Character;
using System.Collections.Generic;
using UnityEngine;


public class EquipmentManager : CharacterComponentBase
{
    [SerializeField]
    private List<EquipmentSlot> slots;
    private PawnEntity pawn;

    public EquipmentSlot this[ESlotType slot]
        => slots.Find(x => x.slotType == slot);

    public EquipmentManager()
    {
        slots = new()
        {
            new EquipmentSlot(ESlotType.Weapon),
            new EquipmentSlot(ESlotType.Armor),
            new EquipmentSlot(ESlotType.Jewelry),
            new EquipmentSlot(ESlotType.SkillBook),
            new EquipmentSlot(ESlotType.SkillBook),
            new EquipmentSlot(ESlotType.SkillBook),
        };
    }

    public void Initialize(PawnEntity pawn)
    {
        this.pawn = pawn;
        foreach (EquipmentSlot slot in slots)
        {
            if (slot.equipment != null)
                slot.equipment.Register(pawn);
        }
    }

    /// <summary>
    /// 获取slotType类型的第index个装备槽
    /// </summary>
    public EquipmentSlot Get(ESlotType slotType, int index)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].slotType == slotType)
            {
                if (index == 0)
                    return slots[i];
                index--;
            }
        }
        throw new System.ArgumentException();
    }

    /// <summary>
    /// 获取slotType类型的第一个空余装备槽（若无空余则返回第一个装备槽）
    /// </summary>
    private EquipmentSlot GetSlot(ESlotType slotType)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].slotType == slotType && slots[i].equipment == null)
            {
                 return slots[i];
            }
        }
        return Get(slotType, 0);
    }

    /// <summary>
    /// 装上装备（返回被卸下的装备）
    /// </summary>
    public Equipment Equip(Equipment equipment)
    {
        Equipment ret = null;
        EquipmentSlot slot = GetSlot(equipment.slotType);
        if (slot.equipment != null)
        {
            ret = slot.equipment;
            slot.equipment.Unregister(pawn);
        }
        equipment.Register(pawn);
        slot.equipment = equipment;
        pawn.RefreshProperty();
        return ret;
    }

    /// <summary>
    /// 卸下装备（返回被卸下的装备）
    /// </summary>
    public Equipment Unequip(ESlotType slotType, int index)
    {
        EquipmentSlot slot = Get(slotType, index);
        if (slot.equipment != null)
        {
            slot.equipment.Unregister(pawn);
            return slot.equipment;
        }
        pawn.RefreshProperty();
        return null;
    }

    public void GetAll(List<Equipment> ret)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].equipment != null)
                ret.Add(slots[i].equipment);
        }
    }

    public void UnEquipAll()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].equipment != null)
                slots[i].equipment.Unregister(pawn);
            slots[i].equipment = null;
        }
        pawn.RefreshProperty();
    }
}
