using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "新装备", menuName = "装备")]
public class Equipment : PawnModifierSO
{
    public ESlotType slotType;
    public GameObject animationPrefab;
    public List<PawnParameterModifier> parameterOnAgent;

    protected override string TypeName => "装备";

    public void ApplyParameter(PawnEntity pawn)
    {
        for (int i = 0; i < parameterOnAgent.Count; i++)
        {
            PawnParameterModifier modifier = parameterOnAgent[i];
            pawn.parameterDict[modifier.ParameterName] += modifier.deltaValue;
        }
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        if(parameterOnAgent.Count > 0)
            DescribeParameter(sb);
    }

    protected virtual void DescribeParameter(StringBuilder sb)
    {
        sb.AppendLine("战斗开始时:");
        for (int i = 0;i < parameterOnAgent.Count;i++)
        {
            parameterOnAgent[i].Describe(sb, "自身");
        }
        sb.AppendLine();
    }
}
