using Character;
using UnityEngine;
using UnityEngine;

[CreateAssetMenu(fileName = "力量", menuName = "词条/力量")]
public class FindStrengthSO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.OffenceComponent.strength;
    }
}
