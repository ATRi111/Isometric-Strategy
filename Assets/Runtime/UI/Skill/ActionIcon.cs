using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionIcon : IconUI, IPointerClickHandler
{
    private IsometricGridManager Igm => IsometricGridManager.Instance;

    private SkillUIManager skillUIManager;

    private Image image;
    private Canvas canvas;
    public PawnAction action;

    public int extraSortingOrder;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        skillUIManager.PreviewAction?.Invoke(action);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        skillUIManager.StopPreviewAction?.Invoke(action);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            skillUIManager.AfterSelectAction?.Invoke(action);
    }

    public void SetAction(PawnAction action)
    {
        info = action.Description;
        this.action = action;
    }

    protected override void Awake()
    {
        base.Awake();
        image = GetComponentInChildren<Image>();
        canvas = GetComponent<Canvas>();
        canvas.overrideSorting = true;
        image.alphaHitTestMinimumThreshold = 0.2f;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        skillUIManager = SkillUIManager.FindInstance();
        canvas.sortingOrder = Igm.CellToSortingOrder(transform.position) + extraSortingOrder;
    }
}
