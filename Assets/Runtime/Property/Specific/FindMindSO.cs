using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "����", menuName = "����/����")]
public class FindMindSO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.OffenceComponent.mind;
    }
}
