using UnityEngine.UI;

public class PlayerIcon : IconUI
{
    private LevelManager levelManager;
    private PlayerManager playerManager;

    public int index;

    public void CheckAvailable()
    {
        if (index < playerManager.playerList.Count)
        {
            canvasGroup.Visible = true;
            image.sprite = playerManager.playerList[index].icon;
            info = playerManager.playerList[index].EntityName;
        }
        else
        {
            canvasGroup.Visible = false;
            info = string.Empty;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        playerManager = PlayerManager.FindInstance();
        levelManager = GetComponentInParent<LevelManager>();
        levelManager.OnReturnToPrepareMenu += CheckAvailable;
        image = GetComponentInParent<Image>();
    }

    private void Start()
    {
        CheckAvailable();
    }
}
