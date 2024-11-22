using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "������", menuName = "����/������")]
public class FindClimbAbilitySO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.MovableGridObject.climbAbility;
    }
}