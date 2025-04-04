using UIExtend;

public class NextPawnButton : ButtonBase
{
    private PawnPanel pawnPanel;

    protected override void OnClick()
    {
        pawnPanel.Next();
    }

    private void Refresh()
    {
        gameObject.SetActive(pawnPanel.pawnList.Count > 1);
    }

    protected override void Awake()
    {
        base.Awake();
        pawnPanel = GetComponentInParent<PawnPanel>();
        pawnPanel.OnRefresh += Refresh;
    }
}
