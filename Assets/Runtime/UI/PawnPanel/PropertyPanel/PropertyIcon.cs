using TMPro;

public class PropertyIcon : IconUI
{
    private TextMeshProUGUI tmp;

    protected override void Awake()
    {
        base.Awake();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        info = PawnPropertyUtility.GetProperty(tmp.text).description;
    }
}
