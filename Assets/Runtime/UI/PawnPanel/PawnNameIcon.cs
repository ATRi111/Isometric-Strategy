using TMPro;

public class PawnNameIcon : InfoIcon
{
    private PawnPanel pawnPanel;
    private TextMeshProUGUI tmp;

    public void Refresh()
    {
        PawnEntity pawn = pawnPanel.SelectedPawn;
        tmp.text = pawn.EntityNameWithColor;
        info = pawn.description;
    }

    protected override void Awake()
    {
        base.Awake();
        pawnPanel = GetComponentInParent<PawnPanel>();
        tmp = GetComponent<TextMeshProUGUI>();
        pawnPanel.RefreshAll += Refresh;
    }
}
