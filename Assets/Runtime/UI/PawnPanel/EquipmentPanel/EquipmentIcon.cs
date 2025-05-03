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
        image.sprite = null;   //TODO:����λͼ��
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
                    info = $"��{EquipmentSlot.SlotTypeName(slot.slotType)}��λ";
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
