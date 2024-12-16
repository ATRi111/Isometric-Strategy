using UIExtend;
using UnityEngine;

public class ActionIconPerspectiveController : MonoBehaviour
{
    private CanvasGroupPlus canvasGroup;
    private ActionIcon icon;
    private PerspectiveController perspectiveController;

    public void EnterPerspectiveMode()
    {
        if (perspectiveController.CoverCheck(icon.action.target + Vector3Int.back))
            canvasGroup.Visible = false;
    }

    public void ExitPerspectiveMode()
    {
        canvasGroup.Visible = true;
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroupPlus>();
        icon = GetComponent<ActionIcon>();
        perspectiveController = IsometricGridManager.FindInstance().PerspectiveController;
    }

    private void OnEnable()
    {
        perspectiveController.EnterPerspectiveMode += EnterPerspectiveMode;
        perspectiveController.ExitPerspectiveMode += ExitPerspectiveMode;
    }

    private void OnDisable()
    {
        perspectiveController.EnterPerspectiveMode -= EnterPerspectiveMode;
        perspectiveController.ExitPerspectiveMode -= ExitPerspectiveMode;
    }
}
