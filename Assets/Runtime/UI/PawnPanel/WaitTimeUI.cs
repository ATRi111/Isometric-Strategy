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
        info = "等待时间变为0后,轮到此角色行动";
    }
}
