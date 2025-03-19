using TMPro;

public class PawnRaceIcon : InfoIcon
{
    private IPawnReference pawnReference;
    private TextMeshProUGUI tmp;

    public void Refresh()
    {
        PawnEntity pawn = pawnReference.CurrentPawn;
        tmp.text = pawn.race.name;
        info = pawn.race.extraDescription;
    }

    protected override void Awake()
    {
        base.Awake();
        pawnReference = GetComponentInParent<IPawnReference>();
        tmp = GetComponent<TextMeshProUGUI>();
        pawnReference.OnRefresh += Refresh;
    }
}
