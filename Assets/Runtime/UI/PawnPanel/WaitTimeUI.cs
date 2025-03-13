using TMPro;
using UnityEngine;

public class WaitTimeUI : MonoBehaviour
{
    private PawnPanel pawnPanel;
    private TextMeshProUGUI tmp;

    private void Refresh()
    {
        tmp.text = $"ʣ��ȴ�ʱ��:{pawnPanel.SelectedPawn.time}";
    }

    protected void Awake()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        pawnPanel = GetComponentInParent<PawnPanel>();
        pawnPanel.RefreshAll += Refresh;
    }
}
