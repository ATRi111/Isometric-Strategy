using UnityEngine;

[CreateAssetMenu(fileName = "新装备", menuName = "装备")]
public class Equipment : PawnModifierSO
{
    public ESlotType slotType;

    protected override string TypeName => "装备";
}
