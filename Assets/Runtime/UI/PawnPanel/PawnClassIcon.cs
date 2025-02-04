using TMPro;

public class PawnClassIcon : IconUI
{
    private PawnPanel pawnPanel;
    private TextMeshProUGUI tmp;

    public void Refresh(PawnEntity pawnEntity)
    {
        tmp.text = pawnEntity.pClass.name;
        message = pawnEntity.pClass.extraDescription;
    }

    protected override void Awake()
    {
        base.Awake();
        pawnPanel = GetComponentInParent<PawnPanel>();
        tmp = GetComponent<TextMeshProUGUI>();
        pawnPanel.RefreshAll += Refresh;
    }
}
