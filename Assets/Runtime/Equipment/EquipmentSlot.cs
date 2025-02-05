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
            ESlotType.Weapon => "武器",
            ESlotType.Armor => "护甲",
            ESlotType.Jewelry => "饰品",
            ESlotType.SkillBook => "技能书",
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
