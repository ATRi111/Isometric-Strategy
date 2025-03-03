using Services;
using Services.Event;
using UIExtend;
using UnityEngine;

public class LoadSceneUI : MonoBehaviour
{
    private IEventSystem eventSystem;
    private CanvasGroupPlus canvasGroup;

    private void AfterLoadScene(int _)
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
        eventSystem.AddListener<int>(EEvent.AfterLoadScene, AfterLoadScene);
        eventSystem.AddListener<int>(EEvent.BeforeUnLoadScene, BeforeUnLoadScene);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<int>(EEvent.AfterLoadScene, AfterLoadScene);
        eventSystem.RemoveListener<int>(EEvent.BeforeUnLoadScene, BeforeUnLoadScene);
    }
}
