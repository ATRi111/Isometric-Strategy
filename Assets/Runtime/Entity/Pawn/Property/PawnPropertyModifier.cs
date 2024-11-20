using Character;

public abstract class PawnPropertyModifier<T> : PropertyModifier<T> where T : struct
{
    public abstract void Bind(PawnEntity pawn);
}
