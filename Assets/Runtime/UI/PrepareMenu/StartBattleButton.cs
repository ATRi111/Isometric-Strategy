using UIExtend;

public class StartBattleButton : ButtonBase
{
    private LevelManager levelManager;

    protected override void OnClick()
    {
        levelManager.StartBattle();
    }

    protected override void Awake()
    {
        base.Awake();
        levelManager = GetComponentInParent<LevelManager>();
    }
}
