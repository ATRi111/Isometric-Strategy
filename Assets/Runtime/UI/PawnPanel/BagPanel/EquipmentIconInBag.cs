using MyTool;
using Services.Event;
using UnityEngine.EventSystems;

public class EquipmentIconInBag : InfoIcon, IPointerClickHandler
{
    private PawnPanel pawnPanel;
    private BagPanel bagPanel;
    private PlayerManager playerManager;

    public int index;

    public void Refresh()
    {
        if (index < bagPanel.visibleEquipments.Count)
        {
            Equipment equipment = bagPanel.visibleEquipments[index];
            image.sprite = equipment.icon;
            canvasGroup.Visible = true;
            info = equipment.name.Bold() + "\n" + equipment.Description;
        }
        else
        {
            canvasGroup.Visible = false;
            info = string.Empty;
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        Equipment equipment = bagPanel.visibleEquipments[index];
        pawnPanel.propertyChangeDict.Clear();
        equipment.MockPropertyChange(true, pawnPanel.propertyChangeDict);
        equipment = bagPanel.currentSlot.equipment;
        if (equipment != null)
            equipment.MockPropertyChange(false, pawnPanel.propertyChangeDict);
        pawnPanel.PreviewPropertyChange?.Invoke();
    }

    protected override void ExtractKeyWords()
    {
        base.ExtractKeyWords();
        Equipment equipment = bagPanel.visibleEquipments[index];
        equipment.ExtractKeyWords(KeyWordList);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Equipment equipment = bagPanel.visibleEquipments[index];
            playerManager.unusedEquipmentList.Remove(equipment);
            Equipment unequipped = bagPanel.equipmentManager.Equip(bagPanel.currentSlot, equipment);
            if (unequipped != null)
                playerManager.unusedEquipmentList.Add(unequipped);
            pawnPanel.OnRefresh?.Invoke();
            if (!string.IsNullOrWhiteSpace(info))
                eventSystem.Invoke(EEvent.ShowInfo, info, normalizedDistance);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        pawnPanel = GetComponentInParent<PawnPanel>();
        pawnPanel.OnRefresh += Refresh;
        playerManager = PlayerManager.FindInstance();
        bagPanel = GetComponentInParent<BagPanel>();
    }
}
