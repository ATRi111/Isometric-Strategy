using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "������", menuName = "����/������")]
public class FindSpeedUpRateSO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.speedUpRate;
    }
}
