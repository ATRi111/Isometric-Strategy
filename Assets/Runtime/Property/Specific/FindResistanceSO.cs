using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "����", menuName = "����/����")]
public class FindResistanceSO : FindPawnPropertySO
{
    public EDamageType damageType;

    public override CharacterProperty FindProperty()
    {
        return pawn.DefenceComponent.resistance[damageType];
    }
}
