using Services;
using UnityEngine;

public class Test : MonoBehaviour
{
    private GameManager gameManager;
    private IsometricGridManager igm;

    private void Awake()
    {
        gameManager = ServiceLocator.Get<GameManager>();
        igm = IsometricGridManager.FindInstance();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            gameManager.StartBattle();
        }
    }
}
