using System.Text;

public enum EParameterConditionType
{
    Equal,
    Greater,
    Less,
    GreaterEqual,
    LessEqual,
    NotEqual,
}

[System.Serializable]
public class ParameterPreCondition
{
    public string ParameterName
        => PawnEntity.ParameterTable.IndexToName(parameterIndex);

    public EParameterConditionType conditionType;
    public int parameterIndex;
    public int value;

    public bool Verify(PawnEntity pawn)
    {
        float current = pawn.parameterDict[ParameterName];
        return conditionType switch
        {
            EParameterConditionType.Equal => current == value,
            EParameterConditionType.Greater => current > value,
            EParameterConditionType.Less => current < value,
            EParameterConditionType.GreaterEqual => current >= value,
            EParameterConditionType.LessEqual => current <= value,
            EParameterConditionType.NotEqual => current != value,
            _ => false
        };
    }

    public void Describe(StringBuilder sb)
    {
        sb.Append(ParameterName);
        string stype = conditionType switch
        {
            EParameterConditionType.Equal => "等于",
            EParameterConditionType.Greater => "大于",
            EParameterConditionType.Less => "小于",
            EParameterConditionType.GreaterEqual => "大于等于",
            EParameterConditionType.LessEqual => "小于等于",
            EParameterConditionType.NotEqual => "不等于",
            _ => string.Empty
        };
        sb.Append(stype);
        sb.Append(value);
        sb.AppendLine();
    }
}