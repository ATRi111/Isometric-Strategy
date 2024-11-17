using MyTool;
using Services;
using Services.ObjectPools;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlanUI : MonoBehaviour
{
    private SkillUIManager skillUIManager;
    private IObjectManager objectManager;
    private IsometricGridManager igm;
    private readonly List<Plan> currentPlans = new();

    private void AfterSelectSkill(Skill skill)
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
        currentPlans.Clear();
        skillUIManager.currentBrain.MakePlan(skill, currentPlans);
        for (int i = 0; i < currentPlans.Count; i++)
        {
            Vector3 world = igm.CellToWorld(currentPlans[i].action.target - Vector3Int.forward);
            IMyObject obj = objectManager.Activate("PlanIcon", world, Vector3.zero, transform);
            obj.Transform.SetLossyScale(Vector3.one);
            PlanIcon icon = obj.Transform.GetComponent<PlanIcon>();
            icon.SetPlan(currentPlans[i]);
        }
    }

    private void AfterSelectPlan(Plan _)
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
    }

    private void Awake()
    {
        skillUIManager = SkillUIManager.FindInstance();
        objectManager = ServiceLocator.Get<IObjectManager>();
        igm = IsometricGridManager.FindInstance();
    }

    private void OnEnable()
    {
        skillUIManager.AfterSelectSkill += AfterSelectSkill;
        skillUIManager.AfterSelectPlan += AfterSelectPlan;
    }

    private void OnDisable()
    {
        skillUIManager.AfterSelectSkill -= AfterSelectSkill;
        skillUIManager.AfterSelectPlan -= AfterSelectPlan;
    }
}
