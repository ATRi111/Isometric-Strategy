using System.Collections.Generic;
using UnityEngine;

public class DebugPlanUIGenerator : MonoBehaviour
{
#if UNITY_EDITOR
    public GameObject prefab;
    private IsometricGridManager igm;
    private IsometricGridManager Igm
    {
        get
        {
            if (igm == null)
                igm = IsometricGridManager.FindInstance();
            return igm;
        }
    }

    public PawnBrain brain;
    public int paintIndex;

    public List<Skill> skills;
    public Dictionary<Vector3Int, Plan> visiblePlans = new();

    private void Filter()
    {
        visiblePlans.Clear();
        if(paintIndex == 0 || paintIndex > skills.Count)
        {
            foreach (Plan plan in brain.plans)
            {
                Vector3Int target = plan.action.target;
                if (!visiblePlans.ContainsKey(target))
                    visiblePlans.Add(target, plan);
                else if (plan.CompareTo(visiblePlans[target]) < 0)
                    visiblePlans[target] = plan;
            }
        }
        else
        {
            foreach (Plan plan in brain.plans)
            {
                Vector3Int target = plan.action.target;
                if (plan.action.skill == skills[paintIndex - 1])
                {
                    visiblePlans.Add(target, plan);
                }
            }
        }
    }

    public void Paint()
    {
        if (brain == null)
            return;
        Clear();
        GameObject obj = new("DebugPlan");
        skills = brain.learnedSkills.list;
        Filter();
        foreach(Plan plan in visiblePlans.Values)
        {
            PaintPlan(plan, obj.transform);
        }
    }

    internal Vector3 WorldPosition(Plan plan)
    {
        return Igm.CellToWorld(plan.action.target - Vector3Int.forward);
    }

    public void Clear()
    {
        GameObject obj = GameObject.Find("DebugPlan");
        Destroy(obj);
    }

    private void PaintPlan(Plan plan, Transform parent)
    {
        GameObject obj = Instantiate(prefab);
        obj.name = plan.action.skill.name;
        obj.transform.SetParent(parent);
        obj.transform.position = WorldPosition(plan);
        obj.GetComponent<DebugPlanUI>().Initialize(plan);
    }
#endif
}
