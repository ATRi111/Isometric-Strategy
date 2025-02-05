using TMPro;

public class ParameterIcon : IconUI
{
    public string parameterName;
    private TextMeshProUGUI tmp;

    public void SetParameter(PawnEntity pawn, string parameterName)
    {
        int value = pawn.parameterDict[parameterName];
        tmp.text = parameterName + ":" + value;
        info = PawnEntity.ParameterTable[parameterName].description;
    }

    protected override void Awake()
    {
        base.Awake();
        tmp = GetComponent<TextMeshProUGUI>();
    }
}
