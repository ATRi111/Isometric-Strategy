using System.Collections.Generic;
using UIExtend;
using UnityEngine;

public class BagPanel : MonoBehaviour
{
    private PawnPanel pawnPanel;
    private CanvasGroupPlus canvasGroup;
    private PlayerManager playerManager;
    private EquipmentIconInBag[] icons;

    public EquipmentManager equipmentManager;
    public EquipmentSlot currentSlot;   //当前需要切换装备的槽位
    public List<Equipment> visibleEquipments = new();

    public void Refresh()
    {
        PawnEntity pawn = pawnPanel.SelectedPawn;
        equipmentManager = pawn.EquipmentManager;
        canvasGroup.Visible = canvasGroup.Visible && equipmentManager.CanChangeEquipment;
        RefreshVisibleEquipments();
    }

    public void RefreshVisibleEquipments()
    {
        visibleEquipments.Clear();
        if (currentSlot != null)
        {
            for (int i = 0; i < playerManager.unusedEquipmentList.Count; i++)
            {
                if (playerManager.unusedEquipmentList[i].slotType == currentSlot.slotType)
                    visibleEquipments.Add(playerManager.unusedEquipmentList[i]);
            }
        }
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].Refresh();
        }
    }

    private void ChangeEquipment(EquipmentSlot slot)
    {
        if (slot == currentSlot)
        {
            canvasGroup.Visible = false;
            currentSlot = null;
        }
        else
        {
            currentSlot = slot;
            canvasGroup.Visible = equipmentManager.CanChangeEquipment;
            RefreshVisibleEquipments();
        }
    }

    private void Awake()
    {
        playerManager = PlayerManager.FindInstance();
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
