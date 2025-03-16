using System.Collections.Generic;
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
}
