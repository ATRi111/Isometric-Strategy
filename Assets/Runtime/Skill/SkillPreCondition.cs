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
    public EConditionType conditionType;
    public string parameterName;
    public int value;

    public bool Verify(PawnEntity pawn)
    {
        float current = pawn.parameterDict[parameterName];
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
}
