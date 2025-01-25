using UIExtend;

public class SelectPlayerToggle : ToggleBase
{
    private PlayerManager playerManager;
    private PlayerIcon playerIcon;

    protected override void OnToggle(bool value)
    {
        if (value)
            playerManager.Select(playerIcon.index);
        else
            playerManager.Unselect(playerIcon.index);
    }

    private void AfterSelectChange()
    {
        Toggle.interactable = !playerManager.FullySelected || Toggle.isOn;
    }

    protected override void Awake()
    {
        base.Awake();
        playerIcon = GetComponentInParent<PlayerIcon>();
        playerManager = PlayerManager.FindInstance();
        playerManager.AfterSelectChange += AfterSelectChange;
    }
}
