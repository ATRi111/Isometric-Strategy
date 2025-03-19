using TMPro;

public class WaitTimeUI : InfoIcon
{
    private IPawnReference pawnReference;
    private TextMeshProUGUI tmp;

    private void Refresh()
    {
        tmp.text = $"{pawnReference.CurrentPawn.time - gameManager.Time}";
    }

    protected override void Awake()
    {
        base.Awake();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        pawnReference = GetComponentInParent<IPawnReference>();
        pawnReference.OnRefresh += Refresh;
        info = "�ȴ�ʱ���Ϊ0��,�ֵ��˽�ɫ�ж�";
    }
}
