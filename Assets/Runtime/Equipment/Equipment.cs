using System.Collections.Generic;
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
}
