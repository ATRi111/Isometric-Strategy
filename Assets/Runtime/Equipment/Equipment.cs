using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "��װ��", menuName = "װ��")]
public class Equipment : PawnModifierSO
{
    public ESlotType slotType;
    public GameObject animationPrefab;
    public List<PawnParameterModifier> parameterOnAgent;

    protected override string TypeName => "װ��";

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
        sb.AppendLine("ս����ʼʱ:");
        for (int i = 0;i < parameterOnAgent.Count;i++)
        {
            parameterOnAgent[i].Describe(sb, "����");
        }
        sb.AppendLine();
    }
}
