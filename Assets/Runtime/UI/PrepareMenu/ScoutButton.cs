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
            tmp.text = "准备完毕";
        }
        else
        {
            levelManager.OnReturnToPrepareMenu?.Invoke();
            isScouting = false;
            tmp.text = "战前准备";
        }
    }

    protected override void Awake()
    {
        base.Awake();
        levelManager = GetComponentInParent<LevelManager>();
        isScouting = false;
    }
}
