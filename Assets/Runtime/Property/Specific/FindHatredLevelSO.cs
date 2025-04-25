using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "嘲讽等级", menuName = "词条/嘲讽等级")]
public class FindHatredLevelSO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.hatredLevel;
    }
}
