using Character;

public class PawnBrain : CharacterComponentBase
{
    public PawnEntity Pawn => entity as PawnEntity;

    public virtual float Evaluate(EffectUnit effectUnit)
    {
        return 0f;
    }
}
