using Services;
using Services.Event;
using UIExtend;
using UnityEngine;

public class AfterBattleUI : MonoBehaviour
{
    protected IEventSystem eventSystem;
    protected CanvasGroupPlus canvasGroup;

    public void Show()
    {
        canvasGroup.Visible = true; 
    }

    protected virtual void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        canvasGroup = GetComponentInChildren<CanvasGroupPlus>();
    }

    protected virtual void OnEnable()
    {
        eventSystem.AddListener(EEvent.AfterBattle, Show);
    }

    protected virtual void OnDisable()
    {
        eventSystem.AddListener(EEvent.AfterBattle, Show);
    }
}
