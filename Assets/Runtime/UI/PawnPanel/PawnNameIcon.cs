using TMPro;

public class PawnNameIcon : InfoIcon
{
    private IPawnReference pawnReference;
    private TextMeshProUGUI tmp;

    public void Refresh()
    {
        PawnEntity pawn = pawnReference.CurrentPawn;
        tmp.text = pawn.EntityNameWithColor;
        info = pawn.description;
    }

    protected override void Awake()
    {
        base.Awake();
        pawnReference = GetComponentInParent<IPawnReference>();
        tmp = GetComponent<TextMeshProUGUI>();
        pawnReference.OnRefresh += Refresh;
    }
}
