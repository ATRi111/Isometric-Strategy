using System;

[Serializable]
public class PawnProperty : ICloneable
{
    public int climbAbility;
    public int dropAbility;
    public int moveAbility;
    public int maxHP;

    public PawnProperty(int climbAbility, int dropAbility, int moveAbility, int maxHP)
    {
        this.climbAbility = climbAbility;
        this.dropAbility = dropAbility;
        this.moveAbility = moveAbility;
        this.maxHP = maxHP;
    }

    public object Clone()
    {
        return new PawnProperty(climbAbility, dropAbility, moveAbility, maxHP);
    }
}
