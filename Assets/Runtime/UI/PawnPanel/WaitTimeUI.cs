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
        info = "�ȴ�ʱ���Ϊ0��,�ֵ��˽�ɫ�ж�";
    }
}
