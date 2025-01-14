using Character;
using System;

public abstract class FindPawnPropertySO : FindPropertySO
{
    [NonSerialized]
    public PawnEntity pawn;

    public string description;
}