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
    public static string SlotTypeName(ESlotType slotType)
    {
        return slotType switch
        {
            ESlotType.Weapon => "����",
            ESlotType.Armor => "����",
            ESlotType.Jewelry => "��Ʒ",
            ESlotType.SkillBook => "������",
            _ => string.Empty,
        };
    }

    public ESlotType slotType;
    public Equipment equipment;

    public EquipmentSlot(ESlotType slotType)
    {
        this.slotType = slotType;
    }
}
