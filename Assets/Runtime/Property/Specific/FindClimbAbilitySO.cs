using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "�ƶ���", menuName = "����/�ƶ���")]
public class FindMoveAbilitySO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.MovableGridObject.moveAbility;
    }
}