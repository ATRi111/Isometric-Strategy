using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "����", menuName = "����/����")]
public class FindDexteritySO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.OffenceComponent.dexterity;
    }
}
