using Services;
using Services.Event;
using UIExtend;
using UnityEngine;

/// <summary>
/// 仅在战斗中（或战斗外）显示的UI
/// </summary>
[RequireComponent(typeof(CanvasGroupPlus))]
public class BattleUI : MonoBehaviour
{
    private IEventSystem eventSystem;
    private CanvasGroupPlus canvasGroup;

    public bool revert;

    private void BeforeBattle()
    {
        canvasGroup.Visible = !revert;
    }

    private void AfterBattle(bool _)
    {
        canvasGroup.Visible = revert;
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        canvasGroup = GetComponent<CanvasGroupPlus>();
    }

    private void OnEnable()
    {
        eventSystem.AddListener(EEvent.BeforeBattle, BeforeBattle);
        eventSystem.AddListener<bool>(EEvent.AfterBattle, AfterBattle);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener(EEvent.BeforeBattle, BeforeBattle);
        eventSystem.RemoveListener<bool>(EEvent.AfterBattle, AfterBattle);
    }
}
