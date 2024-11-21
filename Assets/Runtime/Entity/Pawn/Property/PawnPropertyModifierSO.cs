using UnityEngine;

public class PawnPropertyModifierSO : ScriptableObject
{
    protected PawnPropertyModifier propertyModifier;

    public void Bind(PawnEntity pawn)
        => propertyModifier.Bind(pawn);

    public void Register()
        => propertyModifier.Register();

    public void Unregister()
        => propertyModifier.Unregister();
}
