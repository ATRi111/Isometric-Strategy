using Services;
using Services.Event;
using UIExtend;
using UnityEngine;

public class LoadSceneUI : MonoBehaviour
{
    private IEventSystem eventSystem;
    private CanvasGroupPlus canvasGroup;

    private void AfterMapInitialize()
    {
        canvasGroup.Visible = false;
    }

    private void BeforeUnLoadScene(int _)
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
        eventSystem.AddListener(EEvent.AfterMapInitialize, AfterMapInitialize);
        eventSystem.AddListener<int>(EEvent.BeforeUnLoadScene, BeforeUnLoadScene);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener(EEvent.AfterMapInitialize, AfterMapInitialize);
        eventSystem.RemoveListener<int>(EEvent.BeforeUnLoadScene, BeforeUnLoadScene);
    }
}
