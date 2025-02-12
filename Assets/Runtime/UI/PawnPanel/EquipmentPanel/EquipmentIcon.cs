using MyTool;
using System;

public class EquipmentIcon : IconUI
{
    private PawnPanel pawnPanel;
    [NonSerialized]
    public EquipmentSlot slot;
    
    public int index;

    public void Refresh(PawnEntity pawnEntity)
    {
        image.sprite = null;   //TODO:����λͼ��
        if (index < pawnEntity.EquipmentManager.slots.Count)
        {
            canvasGroup.Visible = true;
            slot = pawnEntity.EquipmentManager.slots[index];
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
        }
    }

    protected override void Awake()
    {
        base.Awake();
        pawnPanel = GetComponentInParent<PawnPanel>();
        pawnPanel.RefreshAll += Refresh;
    }
}
