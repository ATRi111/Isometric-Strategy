using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "最大生命", menuName = "词条/最大生命")]
public class FindMaxHPSO: FindPropertySO
{
    public PawnEntity pawn;
    public override Property FindProperty()
    {
        return pawn.BattleComponent.maxHP;
    }
}