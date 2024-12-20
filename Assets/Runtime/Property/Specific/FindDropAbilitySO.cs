using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "下落力", menuName = "词条/下落力")]
public class FindDropAbilitySO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.MovableGridObject.dropAbility;
    }
}