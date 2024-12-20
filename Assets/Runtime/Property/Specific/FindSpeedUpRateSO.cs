using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "加速率", menuName = "词条/加速率")]
public class FindSpeedUpRateSO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.speedUpRate;
    }
}
