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
    public Action<Plan> AfterSelectPlan;
    public Action AfterCancelSelectPlan;

    public PawnBrain currentBrain;

    private void OnHumanControl(PawnBrain brain)
    {
        if (currentBrain != null)
            throw new InvalidOperationException();

        currentBrain = brain;
        SelectSkill?.Invoke(currentBrain);
    }

    private void ReselectSkill()
    {
        SelectSkill?.Invoke(currentBrain);
    }

    private void ExcutePlan(Plan plan)
    {
        currentBrain.ExcutePlan(plan);
        currentBrain = null;
    }

    private void Awake()
    {
        EventSystem = ServiceLocator.Get<IEventSystem>();
        AfterCancelSelectPlan += ReselectSkill;
        AfterSelectPlan += ExcutePlan;
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
