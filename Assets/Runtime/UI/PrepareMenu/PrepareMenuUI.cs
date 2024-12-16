using Services;
using Services.Event;
using UIExtend;
using UnityEngine;

[RequireComponent(typeof(CanvasGroupPlus))]
public class PrepareMenuUI : MonoBehaviour
{
    protected LevelManager levelManager;
    protected CanvasGroupPlus canvasGroup;
    protected IEventSystem eventSystem;

    protected virtual void OnStartScout()
    {

    }

    protected virtual void OnReturnToPrepareMenu()
    {
        canvasGroup.Visible = true;
    }

    protected virtual void BeforeBattle()
    {
        canvasGroup.Visible = false;
    }

    protected virtual void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        levelManager = GetComponentInParent<LevelManager>();
        canvasGroup = GetComponent<CanvasGroupPlus>();
    }

    protected virtual void OnEnable()
    {
        levelManager.OnStartScout += OnStartScout;
        levelManager.OnReturnToPrepareMenu += OnReturnToPrepareMenu;
        eventSystem.AddListener(EEvent.BeforeBattle, BeforeBattle);
    }

    protected virtual void OnDisable()
    {
        levelManager.OnStartScout -= OnStartScout;
        levelManager.OnReturnToPrepareMenu -= OnReturnToPrepareMenu;
        eventSystem.RemoveListener(EEvent.BeforeBattle, BeforeBattle);
    }
}
