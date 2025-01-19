using TMPro;

public class ParameterIcon : IconUI
{
    public string parameterName;
    public TextMeshProUGUI tmp;

    public void SetParameter(PawnEntity pawn, string parameterName)
    {
        int value = pawn.parameterDict[parameterName];
        tmp.text = parameterName + ":" + value;
    }
}
