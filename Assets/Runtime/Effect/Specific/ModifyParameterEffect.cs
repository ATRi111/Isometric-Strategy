using System.Text;
using UnityEngine;

public class ModifyParameterEffect : Effect
{
    public string parameterName;
    public int value_prev;
    public int value;
    private readonly PawnEntity pawn;
    private readonly Parameter parameter;

    public bool reset;  //表示此效果产生自参数重置

    public override bool Appliable => pawn.parameterDict[parameterName] == value_prev;

    public override bool Revokable => pawn.parameterDict[parameterName] == value;

    public ModifyParameterEffect(Entity victim, string parameterName, int value_prev, int value, int probability = MaxProbability)
        : base(victim, probability)
    {
        this.parameterName = parameterName;
        parameter = PawnEntity.ParameterTable[parameterName];
        pawn = victim as PawnEntity;
        this.value_prev = value_prev;
        this.value = Mathf.Clamp(value, 0, parameter.maxValue);
    }

    public override float ValueFor(PawnEntity pawn)
    {
        return (value - value_prev) * parameter.valuePerUnit * pawn.Sensor.FactionCheck(victim);
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
            sb.Append(parameterName);
            if(reset)
            {
                sb.Append("从");
                sb.Append(value_prev);
                sb.Append("变为");
                sb.Append(value);
            }
            else
            {
                sb.Append("重置为0");
            }
            sb.AppendLine();
        }
    }
}
