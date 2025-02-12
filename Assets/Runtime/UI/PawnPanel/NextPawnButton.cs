using UIExtend;

public class NextPawnButton : ButtonBase
{
    private PawnPanel pawnPanel;

    protected override void OnClick()
    {
        pawnPanel.Previous();
    }

    private void Refresh()
    {
        gameObject.SetActive(pawnPanel.pawnList.Count > 1);
    }

    protected override void Awake()
    {
        base.Awake();
        pawnPanel = GetComponentInParent<PawnPanel>();
        pawnPanel.RefreshAll += Refresh;
    }
}
