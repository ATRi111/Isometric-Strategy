using Services;
using Services.Event;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private IEventSystem eventSystem;
    private SkillUIManager skillUIManager;
    private IsometricGridManager igm;

    private Image image;
    private Canvas canvas;
    public PawnAction action;
    protected string message;
    public int extraSortingOrder;

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
        canvas.overrideSorting = true;
        canvas.sortingOrder = igm.CellToSortingOrder(transform.position) + extraSortingOrder;
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        image = GetComponentInChildren<Image>();
        canvas = GetComponent<Canvas>();
        image.alphaHitTestMinimumThreshold = 0.2f;
    }

    private void OnEnable()
    {
        skillUIManager = SkillUIManager.FindInstance();
        igm = IsometricGridManager.FindInstance();
    }

    private void OnDisable()
    {
        eventSystem.Invoke(EEvent.HideMessage, (object)this);
    }
}
