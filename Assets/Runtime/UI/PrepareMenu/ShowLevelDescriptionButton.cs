using UIExtend;

public class ShowLevelDescriptionButton : ButtonBase
{
    private LevelManager levelManager;

    protected override void OnClick()
    {
        levelManager.ShowLevelDescription?.Invoke();
    }

    protected override void Awake()
    {
        base.Awake();
        levelManager = GetComponentInParent<LevelManager>();
    }
}
