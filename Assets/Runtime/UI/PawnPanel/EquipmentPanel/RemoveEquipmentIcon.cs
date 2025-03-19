using UnityEngine.EventSystems;

public class RemoveEquipmentIcon : InfoIcon , IPointerClickHandler
{
    private PawnPanel pawnPanel;
    private BagPanel bagPanel;
    private PlayerManager playerManager;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        Equipment equipment = bagPanel.currentSlot.equipment;
        if (equipment != null)
            equipment.MockPropertyChange(false, pawnPanel.propertyChangeDict);
        pawnPanel.PreviewPropertyChange?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Equipment unequipped = bagPanel.equipmentManager.Unequip(bagPanel.currentSlot);
            if (unequipped != null)
                playerManager.unusedEquipmentList.Add(unequipped);
            pawnPanel.OnRefresh?.Invoke();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        info = "Ð¶ÔØ×°±¸";
        pawnPanel = GetComponentInParent<PawnPanel>();
        bagPanel = GetComponentInParent<BagPanel>();
        playerManager = PlayerManager.FindInstance();
    }
}
