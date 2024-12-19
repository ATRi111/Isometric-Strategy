using UnityEngine;

public class ActionIconPerspectiveController : PerspectiveController
{
    private ActionIcon icon;

    protected override bool CoverCheck()
    {
        if (icon.action == null)
            return false;
        return perspectiveController.CoverCheck(icon.action.target + Vector3Int.back);
    }

    protected override void Awake()
    {
        base.Awake();
        icon = GetComponent<ActionIcon>();
    }
}
