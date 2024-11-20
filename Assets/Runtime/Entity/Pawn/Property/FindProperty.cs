using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "�������", menuName = "����/�������")]
public class FindMaxHPSO: FindPropertySO
{
    public PawnEntity pawn;
    public override Property FindProperty()
    {
        return pawn.BattleComponent.maxHP;
    }
}