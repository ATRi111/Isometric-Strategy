using UnityEngine;

public class PlayerIcon : MonoBehaviour
{
    private LevelManager levelManager;
    private PlayerManager playerManager;

    public int index;

    public void CheckAvailable()
    {
        gameObject.SetActive(index < playerManager.playerList.Count);
    }

    private void Awake()
    {
        playerManager = PlayerManager.FindInstance();
        levelManager = GetComponentInParent<LevelManager>();
        levelManager.OnReturnToPrepareMenu += CheckAvailable;
    }

    private void Start()
    {
        CheckAvailable();
    }
}
