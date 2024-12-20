using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "÷«¡¶", menuName = "¥ Ãı/÷«¡¶")]
public class FindIntelligenceSO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.OffenceComponent.intelligence;
    }
}
