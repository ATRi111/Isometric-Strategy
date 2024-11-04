[System.Serializable]
public class PawnSetting
{
    public EFaction faction;
    public bool humanControl;

    public PawnSetting(EFaction faction, bool humanControl)
    {
        this.faction = faction;
        this.humanControl = humanControl;
    }
}
