using TMPro;
using UnityEngine;

public class WaitTimeUI : MonoBehaviour
{
    private PawnPanel pawnPanel;
    private TextMeshProUGUI tmp;

    private void Refresh()
    {
        tmp.text = $"Ê£ÓàµÈ´ýÊ±¼ä:{pawnPanel.SelectedPawn.time}";
    }

    protected void Awake()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        pawnPanel = GetComponentInParent<PawnPanel>();
        pawnPanel.RefreshAll += Refresh;
    }
}
