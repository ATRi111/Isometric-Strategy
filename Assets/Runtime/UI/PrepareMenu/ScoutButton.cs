using UIExtend;

public class ScoutButton : ButtonBase
{
    private LevelManager levelManager;
    private bool isScouting;

    protected override void OnClick()
    {
        if (!isScouting)
        {
            levelManager.OnStartScout?.Invoke();
            isScouting = true;
            tmp.text = "Ω· ¯’Ï≤Ï";
        }
        else
        {
            levelManager.OnReturnToPrepareMenu?.Invoke();
            isScouting = false;
            tmp.text = "’Ï≤Ï";
        }
    }

    protected override void Awake()
    {
        base.Awake();
        levelManager = GetComponentInParent<LevelManager>();
        isScouting = false;
    }
}
