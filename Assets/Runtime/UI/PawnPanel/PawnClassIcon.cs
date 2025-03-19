using TMPro;

public class PawnClassIcon : InfoIcon
{
    private IPawnReference pawnRefernce;
    private TextMeshProUGUI tmp;

    public void Refresh()
    {
        PawnEntity pawn = pawnRefernce.CurrentPawn;
        tmp.text = pawn.pClass.name;
        info = pawn.pClass.extraDescription;
    }

    protected override void Awake()
    {
        base.Awake();
        pawnRefernce = GetComponentInParent<IPawnReference>();
        tmp = GetComponent<TextMeshProUGUI>();
        pawnRefernce.OnRefresh += Refresh;
    }
}
