using TMPro;

public class PawnRaceIcon : InfoIcon
{
    private PawnPanel pawnPanel;
    private TextMeshProUGUI tmp;

    public void Refresh()
    {
        PawnEntity pawn = pawnPanel.SelectedPawn;
        tmp.text = pawn.race.name;
        info = pawn.race.extraDescription;
    }

    protected override void Awake()
    {
        base.Awake();
        pawnPanel = GetComponentInParent<PawnPanel>();
        tmp = GetComponent<TextMeshProUGUI>();
        pawnPanel.RefreshAll += Refresh;
    }
}
