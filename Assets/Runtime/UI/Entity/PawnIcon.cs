using Services.Event;
using UnityEngine.EventSystems;

public class PawnIcon : InfoIcon , IPointerClickHandler
{
    public SkillUIManager skillUIManager;
    private PawnEntity pawn;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            eventSystem.Invoke(EEvent.ShowPawnPanel, pawn);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        skillUIManager.PreviewOffenseArea?.Invoke(pawn);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        skillUIManager.StopPreviewOffenseArea?.Invoke(pawn);
    }

    private void AfterSelectSkill(Skill _)
    {
        canvasGroup.Visible = false;
    }

    private void AfterCancelSelectAction()
    {
        canvasGroup.Visible = true;
    }

    private void AfterSelectAction(PawnAction _)
    {
        canvasGroup.Visible = true;
    }

    protected override void Awake()
    {
        base.Awake();
        pawn = GetComponentInParent<PawnEntity>();
        skillUIManager = SkillUIManager.FindInstance();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        skillUIManager.AfterSelectSkill += AfterSelectSkill;
        skillUIManager.AfterCancelSelectAction += AfterCancelSelectAction;
        skillUIManager.AfterSelectAction += AfterSelectAction;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        skillUIManager.AfterSelectSkill -= AfterSelectSkill;
        skillUIManager.AfterCancelSelectAction -= AfterCancelSelectAction;
        skillUIManager.AfterSelectAction -= AfterSelectAction;
    }

    private void Start()
    {
        info = pawn.EntityNameWithColor; //等待名称改完后
    }
}
