using Character;
using UnityEngine;
using UnityEngine;

[CreateAssetMenu(fileName = "����", menuName = "����/����")]
public class FindStrengthSO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.OffenceComponent.strength;
    }
}
