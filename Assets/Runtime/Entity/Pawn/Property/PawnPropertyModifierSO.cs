using UnityEngine;

public class PawnPropertyModifierSO : ScriptableObject
{
    [SerializeField]
    protected PawnPropertyModifier propertyModifier;

    public void Bind(PawnEntity pawn)
        => propertyModifier.Bind(pawn);

    public void Register()
        => propertyModifier.Register();

    public void Unregister()
        => propertyModifier.Unregister();
}
