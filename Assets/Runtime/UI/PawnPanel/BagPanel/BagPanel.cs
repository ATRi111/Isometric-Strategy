using UIExtend;
using UnityEngine;

public class BagPanel : MonoBehaviour
{
    private PawnPanel pawnPanel;
    private CanvasGroupPlus canvasGroup;

    public EquipmentManager equipmentManager;
    public EquipmentSlot currentSlot;

    public void Refresh(PawnEntity pawnEntity)
    {
        equipmentManager = pawnEntity.EquipmentManager;
    }

    private void ChangeEquipment(EquipmentSlot slot)
    {
        if(slot == currentSlot)
        {
            canvasGroup.Visible = false;
            currentSlot = null;
        }
        else
        {
            currentSlot = slot;
            canvasGroup.Visible = equipmentManager.CanChangeEquipment;
        }
    }

    private EquipmentIconInBag[] icons;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroupPlus>();
        pawnPanel = GetComponentInParent<PawnPanel>();
        pawnPanel.RefreshAll += Refresh;
        pawnPanel.ChangeEquipment += ChangeEquipment;
        icons = GetComponentsInChildren<EquipmentIconInBag>();
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].index = i;
        }
    }
}
