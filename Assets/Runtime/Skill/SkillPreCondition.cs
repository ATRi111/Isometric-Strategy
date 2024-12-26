using MyTool;
using System.Text;

public enum EConditionType
{
    Equal,
    Greater,
    Less,
    GreaterEqual,
    LessEqual,
    NotEqual,
}

[System.Serializable]
public class SkillPreCondition
{
    public string ParameterName
        => PawnEntity.ParameterTable.parameters[parameterIndex].name;

    public EConditionType conditionType;
    public int parameterIndex;
    public int value;

    public bool Verify(PawnEntity pawn)
    {
        float current = pawn.parameterDict[ParameterName];
        return conditionType switch
        {
            EConditionType.Equal => current == value,
            EConditionType.Greater => current > value,
            EConditionType.Less => current < value,
            EConditionType.GreaterEqual => current >= value,
            EConditionType.LessEqual => current <= value,
            EConditionType.NotEqual => current != value,
            _ => false
        };
    }

    public void Describe(StringBuilder sb)
    {
        sb.Append(ParameterName.Bold());
        string stype = conditionType switch
        {
            EConditionType.Equal => "等于",
            EConditionType.Greater => "大于",
            EConditionType.Less => "小于",
            EConditionType.GreaterEqual => "大于等于",
            EConditionType.LessEqual => "小于等于",
            EConditionType.NotEqual => "不等于",
            _ => string.Empty
        };
        sb.Append(stype);
        sb.Append(value);
        sb.AppendLine();
    }
}
