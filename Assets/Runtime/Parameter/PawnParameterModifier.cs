using MyTool;
using System.Text;

[System.Serializable]
public class PawnParameterModifier
{
    public string ParameterName
        => PawnEntity.ParameterTable.IndexToName(parameterIndex);

    public int parameterIndex;
    public int deltaValue;

    public void Describe(StringBuilder sb, string victimName)
    {
        sb.Append("Ê¹");
        sb.Append(victimName);
        sb.Append("µÄ");
        sb.Append(ParameterName.Bold());
        sb.Append(deltaValue.ToString("+0;-0"));
        sb.AppendLine();
    }
}