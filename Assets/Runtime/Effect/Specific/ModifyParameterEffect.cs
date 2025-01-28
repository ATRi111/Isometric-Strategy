using MyTool;
using System.Text;

public class ModifyParameterEffect : Effect
{
    public string parameterName;
    public int value_prev;
    public int value;
    private readonly PawnEntity pawn;

    public override bool Appliable => pawn.parameterDict[parameterName] == value_prev;

    public override bool Revokable => pawn.parameterDict[parameterName] == value;

    public ModifyParameterEffect(Entity victim, string parameterName, int value_prev, int value, int probability = 100)
        : base(victim, probability)
    {
        this.parameterName = parameterName;
        this.value_prev = value_prev;
        this.value = value;
        pawn = victim as PawnEntity;
    }

    public override AnimationProcess GenerateAnimation()
    {
        return null;
    }

    public override float ValueFor(PawnEntity pawn)
    {
        return (value - value_prev) * PawnEntity.ParameterTable[parameterName].valuePerUnit * pawn.FactionCheck(victim);
    }

    public override void Apply()
    {
        base.Apply();
        pawn.parameterDict[parameterName] = value;
    }

    public override void Revoke()
    {
        base.Revoke();
        pawn.parameterDict[parameterName] = value_prev;
    }

    public override void Describe(StringBuilder sb, bool result)
    {
        if(value_prev != value)
        {
            base.Describe(sb, result);
            sb.Append("的");
            sb.Append(parameterName.Bold());
            sb.Append("从");
            sb.Append(value_prev);
            sb.Append("变为");
            sb.Append(value);
            sb.AppendLine();
        }
    }
}
