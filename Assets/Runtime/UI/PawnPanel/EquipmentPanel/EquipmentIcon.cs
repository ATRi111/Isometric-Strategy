using MyTool;

public class EquipmentIcon : IconUI
{
    private PawnPanel pawnPanel;
    private EquipmentSlot slot;

    public int index;

    public void Refresh(PawnEntity pawnEntity)
    {
        if (index < pawnEntity.EquipmentManager.slots.Count)
        {
            canvasGroup.Visible = true;
            slot = pawnEntity.EquipmentManager.slots[index];
            if (slot != null)
            {
                if (slot.equipment != null)
                    info = slot.equipment.name.Bold() + "\n" + slot.equipment.Description;
                else
                    info = $"¿Õ{EquipmentSlot.SlotTypeName(slot.slotType)}À¸Î»";
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
