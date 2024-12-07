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

    private void OnHumanControl(PawnEntity pawn)
    {
        if (currentPawn != null)
            throw new Exception($"{currentPawn.gameObject.name}没有行动完,就轮到{pawn.gameObject.name}行动");

        currentPawn = pawn;
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
        EventSystem.AddListener<PawnEntity>(EEvent.OnHumanControl, OnHumanControl);
    }

    private void OnDisable()
    {
        EventSystem.RemoveListener<PawnEntity>(EEvent.OnHumanControl, OnHumanControl);
    }
}
