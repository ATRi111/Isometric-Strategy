using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "�������", menuName = "����/�������")]
public class FindMaxHPSO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.BattleComponent.maxHP;
    }
}