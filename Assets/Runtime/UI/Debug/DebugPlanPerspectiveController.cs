using UnityEngine;

public class DebugPlanPerspectiveController : PerspectiveController
{
    private DebugPlanUI ui;

    protected override bool CoverCheck()
    {
        if (ui.plan == null)
            return false;
        return perspectiveManager.CoverCheck(ui.plan.action.target + Vector3Int.back);
    }

    protected override void Awake()
    {
        base.Awake();
        ui = GetComponent<DebugPlanUI>();
    }
}
