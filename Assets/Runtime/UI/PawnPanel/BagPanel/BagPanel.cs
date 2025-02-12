using UIExtend;
using UnityEngine;

public class BagPanel : MonoBehaviour
{
    private PawnPanel pawnPanel;
    private CanvasGroupPlus canvasGroup;
    private EquipmentManager equipmentManager;
    private EquipmentSlot currentSlot;

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


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroupPlus>();
        pawnPanel = GetComponentInParent<PawnPanel>();
        pawnPanel.RefreshAll += Refresh;
        pawnPanel.ChangeEquipment += ChangeEquipment;
    }

}
