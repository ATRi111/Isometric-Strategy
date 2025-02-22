using Character;
using System.Collections.Generic;


public class EquipmentManager : CharacterComponentBase
{
    public List<EquipmentSlot> slots;
    private PawnEntity pawn;
    public bool CanChangeEquipment => !pawn.GameManager.inBattle && pawn.Brain.humanControl;

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

    public void Register(PawnEntity pawn)
    {
        this.pawn = pawn;
        foreach (EquipmentSlot slot in slots)
        {
            if (slot.equipment != null)
                slot.equipment.Register(pawn);
        }
    }

    public void Unregister(PawnEntity pawn)
    {
        this.pawn = pawn;
        foreach (EquipmentSlot slot in slots)
        {
            if (slot.equipment != null)
                slot.equipment.Unregister(pawn);
        }
    }

    /// <summary>
    /// 装上装备（返回被卸下的装备）
    /// </summary>
    public Equipment Equip(EquipmentSlot slot, Equipment equipment)
    {
        Equipment ret = null;
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

    // 获取slotType类型的第一个空余装备槽（若无空余则返回第一个装备槽）
    private EquipmentSlot GetSlot(ESlotType slotType)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].slotType == slotType && slots[i].equipment == null)
            {
                return slots[i];
            }
        }
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].slotType == slotType)
            {
                return slots[i];
            }
        }
        return null;
    }

    public Equipment Equip(Equipment equipment)
        => Equip(GetSlot(equipment.slotType), equipment);

    /// <summary>
    /// 卸下装备（返回被卸下的装备）
    /// </summary>
    public Equipment Unequip(EquipmentSlot slot)
    {
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
