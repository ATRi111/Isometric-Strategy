using TMPro;

public class PropertyIcon : InfoIcon
{
    private TextMeshProUGUI tmp;

    protected override void Awake()
    {
        base.Awake();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        info = PawnPropertyUtility.GetProperty(tmp.text).description;
    }
}
