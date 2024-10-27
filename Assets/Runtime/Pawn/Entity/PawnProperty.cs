using System;

[Serializable]
public class PawnProperty : ICloneable
{
    public int climbAbility;
    public int dropAbility;
    public int moveAbility;
    public int maxHP;
    public int actionTime;

    public PawnProperty(int climbAbility, int dropAbility, int moveAbility, int maxHP,int actionTime)
    {
        this.climbAbility = climbAbility;
        this.dropAbility = dropAbility;
        this.moveAbility = moveAbility;
        this.maxHP = maxHP;
        this.actionTime = actionTime;
    }

    public object Clone()
    {
        return new PawnProperty(climbAbility, dropAbility, moveAbility, maxHP, actionTime);
    }
}
