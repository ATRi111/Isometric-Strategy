using Services;
using UIExtend;

public class ConfirmButton : ButtonBase
{
    private CanvasGroupPlus canvasGroup;
    private GameManager gameManager;
    private PlayerManager playerManager;

    protected override void OnClick()
    {
        canvasGroup.Visible = false;
        Reward reward = GetComponentInParent<RewardUI>().reward;
        if (reward != null)
            reward.Apply(playerManager);
        gameManager.LoadNextLevel();
    }

    protected override void Awake()
    {
        base.Awake();
        canvasGroup = GetComponentInParent<CanvasGroupPlus>();
        gameManager = ServiceLocator.Get<GameManager>();
        playerManager = PlayerManager.FindInstance();
    }
}
