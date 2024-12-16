using Services;
using Services.Event;
using System;
using UnityEngine;

public class SkillUIManager : MonoBehaviour
{
    public static SkillUIManager FindInstance()
    {
        return GameObject.Find(nameof(SkillUIManager)).GetComponent<SkillUIManager>();
    }

    public IEventSystem EventSystem { get; private set; }
    public Action<PawnEntity> SelectSkill;
    public Action<Skill> AfterSelectSkill;
    public Action<PawnAction> AfterSelectAction;
    public Action AfterCancelSelectAction;
    public Action<PawnAction> PreviewAction;
    public Action<PawnAction> StopPreviewAction;

    public PawnEntity currentPawn;

    private void BeforeDoAction(PawnEntity pawn)
    {
        currentPawn = pawn;
        if (pawn.Brain.humanControl)
            SelectSkill?.Invoke(currentPawn);
    }

    private void ReselectSkill()
    {
        SelectSkill?.Invoke(currentPawn);
    }

    private void ExecuteAction(PawnAction action)
    {
        currentPawn.Brain.ExecuteAction(action);
        currentPawn = null;
    }

    private void Awake()
    {
        EventSystem = ServiceLocator.Get<IEventSystem>();
        AfterCancelSelectAction += ReselectSkill;
        AfterSelectAction += ExecuteAction;
    }

    private void OnEnable()
    {
        EventSystem.AddListener<PawnEntity>(EEvent.BeforeDoAction, BeforeDoAction);
    }

    private void OnDisable()
    {
        EventSystem.RemoveListener<PawnEntity>(EEvent.BeforeDoAction, BeforeDoAction);
    }
}
