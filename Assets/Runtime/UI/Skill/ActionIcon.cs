using Services;
using Services.Event;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    private PawnAction action;
    private IEventSystem eventSystem;
    protected string message;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(message))
            eventSystem.Invoke(EEvent.ShowMessage, (object)this, eventData.position, message);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventSystem.Invoke(EEvent.HideMessage, (object)this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SkillUIManager.FindInstance().AfterSelectAction?.Invoke(action);
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

    private void OnDisable()
    {
        eventSystem.Invoke(EEvent.HideMessage, (object)this);
    }
}
