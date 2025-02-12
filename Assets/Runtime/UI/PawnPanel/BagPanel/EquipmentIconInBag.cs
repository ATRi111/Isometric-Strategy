using MyTool;
using UnityEngine.EventSystems;

public class EquipmentIconInBag : IconUI , IPointerClickHandler
{
    private PawnPanel pawnPanel;
    private PlayerManager playerManager;
    private BagPanel bagPanel;

    public int index;

    public void Refresh(PawnEntity pawnEntity)
    {
        image.sprite = null;   //TODO:ø’ŒªÕº±Í
        if (index < playerManager.unusedEquipmentList.Count)
        {
            Equipment equipment = playerManager.unusedEquipmentList[index];
            canvasGroup.Visible = true;
            info = equipment.name.Bold() + "\n" + equipment.Description;
        }
        else
        {
            canvasGroup.Visible = false;
            info = string.Empty;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Equipment equipment = playerManager.unusedEquipmentList[index];
            playerManager.unusedEquipmentList.RemoveAt(index);
            Equipment unequipped = bagPanel.equipmentManager.Equip(bagPanel.currentSlot, equipment);
            if (unequipped != null)
                playerManager.unusedEquipmentList.Add(unequipped);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        pawnPanel = GetComponentInParent<PawnPanel>();
        pawnPanel.RefreshAll += Refresh;
        playerManager = PlayerManager.FindInstance();
        bagPanel = GetComponentInParent<BagPanel>();
    }
}
