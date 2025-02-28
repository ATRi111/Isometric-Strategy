using Services;
using Services.Event;
using UIExtend;
using UnityEngine;

public class AfterBattleUI : MonoBehaviour
{
    protected IEventSystem eventSystem;
    protected CanvasGroupPlus canvasGroup;

    public bool visibleOnFail;

    public void Show()
    {
        canvasGroup.Visible = true; 
    }

    private void AfterBattle(bool win)
    {
        if (win && !visibleOnFail || !win && visibleOnFail)
            Show();
    }

    protected virtual void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        canvasGroup = GetComponentInChildren<CanvasGroupPlus>();
    }

    protected virtual void OnEnable()
    {
        eventSystem.AddListener<bool>(EEvent.AfterBattle, AfterBattle);
    }

    protected virtual void OnDisable()
    {
        eventSystem.AddListener<bool>(EEvent.AfterBattle, AfterBattle);
    }
}
