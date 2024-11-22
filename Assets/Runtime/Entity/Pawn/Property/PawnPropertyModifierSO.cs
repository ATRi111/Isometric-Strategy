using UnityEngine;

public class PawnPropertyModifierSO : ScriptableObject
{
    [SerializeField]
    protected PawnPropertyModifier propertyModifier;

    public virtual void Register(PawnEntity pawn)
        => propertyModifier.Register(pawn);

    public virtual void Unregister(PawnEntity pawn)
        => propertyModifier.Unregister(pawn);
}
