using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "基础行动时间", menuName = "词条/基础行动时间")]
public class FindActionTimeSO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.actionTime;
    }
}