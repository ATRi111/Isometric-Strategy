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
            PawnEntity pawn = playerManager.playerList[index];
            info = pawn.EntityName + " " + pawn.pClass.name;

        }
        else
        {
            canvasGroup.Visible = false;
            info = string.Empty;
        }
    }

    protected override void ExtractKeyWords()
    {
        base.ExtractKeyWords();
        PawnEntity pawn = playerManager.playerList[index];
        keyWordList.Push(pawn.pClass.name, pawn.pClass.Description);
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
