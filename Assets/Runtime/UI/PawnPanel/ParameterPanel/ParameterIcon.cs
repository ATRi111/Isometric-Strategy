using MyTool;
using System.Text;
using TMPro;

public class ParameterIcon : InfoIcon
{
    public string parameterName;
    private TextMeshProUGUI tmp;

    public void SetParameter(PawnEntity pawn, string parameterName)
    {
        int value = pawn.parameterDict[parameterName];
        tmp.text = value.ToString();
        StringBuilder sb = new();
        sb.AppendLine(parameterName.Bold());
        sb.AppendLine(PawnEntity.ParameterTable[parameterName].description);
        info = sb.ToString();
        image.sprite = PawnEntity.ParameterTable[parameterName].icon;
    }

    protected override void Awake()
    {
        base.Awake();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }
}
