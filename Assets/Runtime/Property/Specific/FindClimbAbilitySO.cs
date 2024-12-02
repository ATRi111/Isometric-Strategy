using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "移动力", menuName = "词条/移动力")]
public class FindMoveAbilitySO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.MovableGridObject.moveAbility;
    }
}