using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "����ȼ�", menuName = "����/����ȼ�")]
public class FindHatredLevelSO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.hatredLevel;
    }
}
