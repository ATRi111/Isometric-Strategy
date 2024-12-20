using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "����", menuName = "����/����")]
public class FindIntelligenceSO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.OffenceComponent.intelligence;
    }
}
