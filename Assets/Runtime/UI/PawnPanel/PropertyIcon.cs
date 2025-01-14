using TMPro;

public class PropertyIcon : IconUI
{
    private TextMeshProUGUI tmp;

    protected override void Awake()
    {
        base.Awake();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        message = PawnPropertyUtility.GetProperty(tmp.text).description;
    }
}
