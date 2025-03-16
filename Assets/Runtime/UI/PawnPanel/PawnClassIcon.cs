using TMPro;

public class PawnClassIcon : InfoIcon
{
    private PawnPanel pawnPanel;
    private TextMeshProUGUI tmp;

    public void Refresh()
    {
        PawnEntity pawn = pawnPanel.SelectedPawn;
        tmp.text = pawn.pClass.name;
        info = pawn.pClass.extraDescription;
    }

    protected override void Awake()
    {
        base.Awake();
        pawnPanel = GetComponentInParent<PawnPanel>();
        tmp = GetComponent<TextMeshProUGUI>();
        pawnPanel.RefreshAll += Refresh;
    }
}
