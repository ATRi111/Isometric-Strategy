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

    private EquipmentSlot FirstEmpty(ESlotType slotType)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].slotType == slotType && slots[i].equipment == null)
            {
                 return slots[i];
            }
        }
        return null;
    }

    public void Equip(Equipment equipment)
    {
        EquipmentSlot slot = FirstEmpty(equipment.slotType) ?? Get(equipment.slotType, 0);
        if (slot.equipment != null)
            slot.equipment.Unregister(pawn);
        equipment.Register(pawn);
        slot.equipment = equipment;
    }

    public void Unequip(ESlotType slotType, int index)
    {
        EquipmentSlot slot = Get(slotType, index);
        if (slot.equipment != null)
            slot.equipment.Unregister(pawn);
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
    }
}
