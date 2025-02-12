using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentToSelect : MonoBehaviour , IPointerClickHandler
{
    private PawnPanel pawnPanel;
    private EquipmentIcon icon;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (icon.slot != null)
            pawnPanel.ChangeEquipment?.Invoke(icon.slot);
    }

    private void Awake()
    {
        pawnPanel = GetComponentInParent<PawnPanel>();
        icon = GetComponent<EquipmentIcon>();
    }
}
