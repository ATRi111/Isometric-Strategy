using MyTool;
using TMPro;
using UnityEngine;

public class PawnNameUI : MonoBehaviour
{
    private PawnPanel pawnPanel;
    private TextMeshProUGUI tmp;

    public void Refresh()
    {
        PawnEntity pawn = pawnPanel.SelectedPawn;
        string color = pawn.faction switch
        {
            EFaction.Ally => FontUtility.SpringGreen3,
            EFaction.Enemy => FontUtility.Red,
            _ => FontUtility.Black
        };
        tmp.text = pawn.EntityName.ColorText(color);
    }

    private void Awake()
    {
        pawnPanel = GetComponentInParent<PawnPanel>();
        tmp = GetComponent<TextMeshProUGUI>();
        pawnPanel.RefreshAll += Refresh;
    }
}
