using Services;
using Services.Event;
using UIExtend;
using UnityEngine;

public class TaskTargetUI : MonoBehaviour
{
    private CanvasGroupPlus canvasGroup;
    private IEventSystem eventSystem;

    private void BeforeBattle()
    {
        canvasGroup.Visible = false;
    }

    private void Awake()
    {
        canvasGroup = GetComponentInChildren<CanvasGroupPlus>();
        eventSystem = ServiceLocator.Get<IEventSystem>();
    }

    private void OnEnable()
    {
        eventSystem.AddListener(EEvent.BeforeBattle, BeforeBattle);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener(EEvent.BeforeBattle, BeforeBattle);
    }
}
