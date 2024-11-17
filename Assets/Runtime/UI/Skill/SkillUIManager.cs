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
    public Action<Skill> AfterSelectSkill;
    public Action<Plan> AfterSelectPlan;
    public Action AfterCancelSelectTarget;

    public PawnBrain currentBrain;

    private void OnHumanControl(PawnBrain brain)
    {
        if (currentBrain != null)
            throw new InvalidOperationException();

        currentBrain = brain;
    }

    private void CancelSelectSkill()
    {
        EventSystem.Invoke(EEvent.OnHumanControl, currentBrain);
    }

    private void SelectPlan(Plan plan)
    {
        currentBrain.ExcutePlan(plan);
        currentBrain = null;
    }

    private void Awake()
    {
        EventSystem = ServiceLocator.Get<IEventSystem>();
        AfterCancelSelectTarget += CancelSelectSkill;
        AfterSelectPlan += SelectPlan;
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
