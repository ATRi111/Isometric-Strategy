[System.Serializable]
public class PawnParameterModifier
{
    public string ParameterName
        => PawnEntity.ParameterTable.parameters[parameterIndex].name;

    public int parameterIndex;
    public int deltaValue;

}