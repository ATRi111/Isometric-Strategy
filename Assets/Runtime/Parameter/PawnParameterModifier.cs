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
        if (deltaValue != 0)
        {
            sb.Append("Ê¹");
            sb.Append(victimName);
            sb.Append("µÄ");
            sb.Append(ParameterName);
            sb.Append(deltaValue.ToString("+0;-0"));
            sb.AppendLine();
        }
    }
}