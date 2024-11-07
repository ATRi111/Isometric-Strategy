using MyTimer;
using Services;
using UnityEngine;

public class Test : MonoBehaviour
{
    private GameManager gameManager;
    private AnimationManager animationManager;
    private IsometricGridManager igm;
    public TimerOnly timer;

    private void Awake()
    {
        gameManager = ServiceLocator.Get<GameManager>();
        animationManager = ServiceLocator.Get<AnimationManager>();
        igm = IsometricGridManager.FindInstance();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.K))
        {
            gameManager.StartBattle();
        }
    }
}
