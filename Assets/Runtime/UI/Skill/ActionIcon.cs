using Services.Event;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionIcon : InfoIcon, IPointerClickHandler
{
    private IsometricGridManager Igm => IsometricGridManager.Instance;

    private SkillUIManager skillUIManager;

    private Canvas canvas;
    public PawnAction action;

    public int extraSortingOrder;

    protected override void ExtractKeyWords()
    {
        base.ExtractKeyWords();
        action.ExtractKeyWords(KeyWordList);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        skillUIManager.PreviewAction?.Invoke(action);
        eventSystem.Invoke(EEvent.SetPawnTaregt, action.FirstPawnVictim);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        skillUIManager.StopPreviewAction?.Invoke(action);
        eventSystem.Invoke<PawnEntity>(EEvent.SetPawnTaregt, null);
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
        Vector3 cell = action.target - action.agent.GridObject.CellPosition;
        Vector2 world = Igm.CellToWorld(cell);
        if (world == Vector2.zero)
            world = Vector2.down;
        else
            world = world.normalized;
        infoPivot = 0.5f * (Vector2.one - world);
        infoOffset = infoOffset.magnitude * world;
    }

    protected override void Awake()
    {
        base.Awake();
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
