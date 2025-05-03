using MyTool;
using System;

public class EquipmentIcon : InfoIcon
{
    private IPawnReference pawnReference;
    [NonSerialized]
    public EquipmentSlot slot;

    public int index;

    protected override void ExtractKeyWords()
    {
        base.ExtractKeyWords();
        if (slot.equipment != null)
            slot.equipment.ExtractKeyWords(KeyWordList);
    }

    public void Refresh()
    {
        PawnEntity pawn = pawnReference.CurrentPawn;
        image.sprite = null;   //TODO:ø’¿∏ŒªÕº±Í
        if (index < pawn.EquipmentManager.slots.Count)
        {
            canvasGroup.Visible = true;
            slot = pawn.EquipmentManager.slots[index];
            if (slot != null)
            {
                if (slot.equipment != null)
                {
                    info = slot.equipment.name.Bold() + "\n" + slot.equipment.Description;
                    image.sprite = slot.equipment.icon;
                }
                else
                    info = $"ø’{EquipmentSlot.SlotTypeName(slot.slotType)}¿∏Œª";
            }
        }
        else
        {
            canvasGroup.Visible = false;
            info = string.Empty;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        pawnReference = GetComponentInParent<IPawnReference>();
        pawnReference.OnRefresh += Refresh;
    }
}
