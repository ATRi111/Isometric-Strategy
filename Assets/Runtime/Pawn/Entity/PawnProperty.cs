[System.Serializable]
public struct PawnProperty
{
    public int climbAbility;
    public int dropAbility;
    public float moveAbility;

    public PawnProperty(int climbAbility = 2, int dropAbility = 3, int moveAbility = 5)
    {
        this.climbAbility = climbAbility;
        this.dropAbility = dropAbility;
        this.moveAbility = moveAbility;
    }
}
