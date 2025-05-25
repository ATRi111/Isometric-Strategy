using Services.Event;
using UIExtend;
using UnityEngine.EventSystems;

public class StartBattleButton : ButtonBase
{
    private LevelManager levelManager;
    private PlayerManager playerManager;

    protected override void OnClick()
    {
        levelManager.StartBattle();
    }

    private void AfterSelectChange()
    {
        Button.interactable = playerManager.SelectedCount == playerManager.MaxSelectedCount;
#if UNITY_EDITOR
        Button.interactable = true;
#endif
    }

    protected override void Awake()
    {
        base.Awake();
        levelManager = GetComponentInParent<LevelManager>();
        playerManager = PlayerManager.FindInstance();
        playerManager.AfterSelectChange += AfterSelectChange;
    }
}
