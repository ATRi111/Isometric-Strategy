using EditorExtend.GridEditor;
using UIExtend;
using UnityEngine;

public class ActionIconPerspectiveController : MonoBehaviour
{
    private CanvasGroupPlus canvasGroup;
    private ActionIcon icon;
    private IsometricGridManager igm;
    private PerspectiveController perspectiveController;

    public bool CoverCheck()
    {
        Vector3Int p = icon.action.target + Vector3Int.back + IsometricGridManager.CoverVector;    //技能目标总是位于地面以上一层
        while (igm.MaxLayerDict.ContainsKey((Vector2Int)p) && p.z >= 0)
        {
            GridObject gridObject = igm.GetObject(p);
            if (gridObject != null && gridObject.GroundHeight > 0)
                return true;
            p += IsometricGridManager.CoverVector;
        }
        return false;
    }

    public void EnterPerspectiveMode()
    {
        if (CoverCheck())
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
        igm = IsometricGridManager.FindInstance();
        perspectiveController = igm.PerspectiveController;
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
