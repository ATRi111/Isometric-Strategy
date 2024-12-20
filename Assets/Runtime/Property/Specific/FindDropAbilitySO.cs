using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "������", menuName = "����/������")]
public class FindDropAbilitySO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.MovableGridObject.dropAbility;
    }
}