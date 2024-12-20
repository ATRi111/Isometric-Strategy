using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "¡È«…", menuName = "¥ Ãı/¡È«…")]
public class FindDexteritySO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.OffenceComponent.dexterity;
    }
}
