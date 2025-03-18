using TMPro;

public class WaitTimeUI : InfoIcon
{
    private PawnPanel pawnPanel;
    private TextMeshProUGUI tmp;

    private void Refresh()
    {
        tmp.text = $"{pawnPanel.SelectedPawn.time - gameManager.Time}";
    }

    protected override void Awake()
    {
        base.Awake();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        pawnPanel = GetComponentInParent<PawnPanel>();
        pawnPanel.RefreshAll += Refresh;
        info = "等待时间变为0后,轮到此角色行动";
    }
}
