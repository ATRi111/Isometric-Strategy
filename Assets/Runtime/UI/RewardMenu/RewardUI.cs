using Services;
using Services.Event;
using UIExtend;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
    private IEventSystem eventSystem;
    private CanvasGroupPlus canvasGroup;
    public Reward reward;

    public void Show()
    {
        canvasGroup.Visible = true; 
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        canvasGroup = GetComponentInChildren<CanvasGroupPlus>();
    }

    private void OnEnable()
    {
        eventSystem.AddListener(EEvent.AfterBattle, Show);
    }

    private void OnDisable()
    {
        eventSystem.AddListener(EEvent.AfterBattle, Show);
    }
}
