using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "精神", menuName = "词条/精神")]
public class FindMindSO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.OffenceComponent.mind;
    }
}
