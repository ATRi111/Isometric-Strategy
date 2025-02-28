using Services.Event;
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

    public void AfterBattle(bool _)
    {
        Toggle.isOn = false;    //修改Toggle的值会导致角色被回收
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
        playerManager = PlayerManager.FindInstance();
        playerManager.AfterSelectChange += CheckInteractable;
    }

    private void OnEnable()
    {
        eventSystem.AddListener<bool>(EEvent.AfterBattle, AfterBattle);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<bool>(EEvent.AfterBattle, AfterBattle);
    }
}
