using UnityEngine;

[CreateAssetMenu(fileName = "��װ��", menuName = "װ��")]
public class Equipment : PawnModifierSO
{
    public ESlotType slotType;
    public GameObject animationPrefab;

    protected override string TypeName => "װ��";
}
