using Services;
using Services.Event;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    public PawnAction action;
    private IEventSystem eventSystem;
    private SkillUIManager skillUIManager;
    protected string message;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(message))
            eventSystem.Invoke(EEvent.ShowMessage, (object)this, eventData.position, message);
        skillUIManager.PreviewAction?.Invoke(action);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventSystem.Invoke(EEvent.HideMessage, (object)this);
        skillUIManager.StopPreviewAction?.Invoke(action);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        skillUIManager.AfterSelectAction?.Invoke(action);
    }

    public void SetAction(PawnAction action)
    {
        message = action.ToString();
        this.action = action;
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        eventSystem = ServiceLocator.Get<IEventSystem>();
        image.alphaHitTestMinimumThreshold = 0.2f;
    }

    private void OnEnable()
    {
        skillUIManager = SkillUIManager.FindInstance();
    }

    private void OnDisable()
    {
        eventSystem.Invoke(EEvent.HideMessage, (object)this);
    }
}
