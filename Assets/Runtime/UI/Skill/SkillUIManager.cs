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
    public Action<PawnBrain> SelectSkill;
    public Action<Skill> AfterSelectSkill;
    public Action<PawnAction> AfterSelectAction;
    public Action AfterCancelSelectAction;
    public Action<PawnAction> PreviewAction;
    public Action<PawnAction> StopPreviewAction;

    public PawnBrain currentBrain;

    private void OnHumanControl(PawnBrain brain)
    {
        if (currentBrain != null)
            throw new Exception($"{currentBrain.Pawn.gameObject.name}没有行动完,就轮到{brain.Pawn.gameObject.name}行动");

        currentBrain = brain;
        SelectSkill?.Invoke(currentBrain);
    }

    private void ReselectSkill()
    {
        SelectSkill?.Invoke(currentBrain);
    }

    private void ExcuteAction(PawnAction action)
    {
        currentBrain.ExcuteAction(action);
        currentBrain = null;
    }

    private void Awake()
    {
        EventSystem = ServiceLocator.Get<IEventSystem>();
        AfterCancelSelectAction += ReselectSkill;
        AfterSelectAction += ExcuteAction;
    }

    private void OnEnable()
    {
        EventSystem.AddListener<PawnBrain>(EEvent.OnHumanControl, OnHumanControl);
    }

    private void OnDisable()
    {
        EventSystem.RemoveListener<PawnBrain>(EEvent.OnHumanControl, OnHumanControl);
    }
}
