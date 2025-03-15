using TMPro;

public class WaitTimeUI : IconUI
{
    private PawnPanel pawnPanel;
    private TextMeshProUGUI tmp;

    private void Refresh()
    {
        tmp.text = $"{pawnPanel.SelectedPawn.time}";
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
