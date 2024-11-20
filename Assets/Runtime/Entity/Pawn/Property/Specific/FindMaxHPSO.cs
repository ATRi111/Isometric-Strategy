using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "最大生命", menuName = "词条/最大生命")]
public class FindMaxHPSO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.BattleComponent.maxHP;
    }
}