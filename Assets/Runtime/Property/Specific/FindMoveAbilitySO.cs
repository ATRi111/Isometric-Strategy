using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "≈ ≈¿¡¶", menuName = "¥ Ãı/≈ ≈¿¡¶")]
public class FindClimbAbilitySO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.MovableGridObject.climbAbility;
    }
}