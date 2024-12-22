using System;

public enum ESlotType
{
    Weapon,
    Armor,
    Jewelry,
    SkillBook
}

[Serializable]
public class EquipmentSlot
{
    public ESlotType slotType;
    public Equipment equipment;

    public EquipmentSlot(ESlotType slot)
    {
        this.slotType = slot;
    }
}
