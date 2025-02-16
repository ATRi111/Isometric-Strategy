using UIExtend;

public class SelectPlayerToggle : ToggleBase
{
    private PlayerManager playerManager;
    private LevelManager levelManager;
    private PlayerIcon playerIcon;

    protected override void OnToggle(bool value)
    {
        if (value)
            playerManager.Select(playerIcon.index);
        else
            playerManager.Unselect(playerIcon.index);
    }

    public void Refresh()
    {
        Toggle.isOn = false;
        CheckInteractable();
    }

    private void CheckInteractable()
    {
        Toggle.interactable = !playerManager.FullySelected || Toggle.isOn;
    }

    protected override void Awake()
    {
        base.Awake();
        playerIcon = GetComponentInParent<PlayerIcon>();
        levelManager = GetComponentInParent<LevelManager>();
        levelManager.OnReturnToPrepareMenu += Refresh;
        playerManager = PlayerManager.FindInstance();
        playerManager.AfterSelectChange += CheckInteractable;
    }
}
