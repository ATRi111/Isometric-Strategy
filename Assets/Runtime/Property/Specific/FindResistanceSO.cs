using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "øπ–‘", menuName = "¥ Ãı/øπ–‘")]
public class FindResistanceSO : FindPawnPropertySO
{
    public EDamageType damageType;

    public override CharacterProperty FindProperty()
    {
        return pawn.DefenceComponent.resistance[damageType];
    }
}
