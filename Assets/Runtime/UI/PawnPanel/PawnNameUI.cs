using MyTool;
using TMPro;
using UnityEngine;

public class PawnNameUI : MonoBehaviour
{
    private PawnPanel pawnPanel;
    private TextMeshProUGUI tmp;

    public void Refresh(PawnEntity pawnEntity)
    {
        string color = pawnEntity.faction switch
        {
            EFaction.Ally => "#4EEE94",
            EFaction.Enemy => "red",
            _ => "black"
        };
        tmp.text = pawnEntity.EntityName.ColorText(color);
    }

    private void Awake()
    {
        pawnPanel = GetComponentInParent<PawnPanel>();
        tmp = GetComponent<TextMeshProUGUI>();
        pawnPanel.RefreshAll += Refresh;
    }
}
