using EditorExtend.GridEditor;
using UnityEngine;

public class GroundPerspectiveController : SpriteController
{
    private IsometricGridManager igm;
    private GridObject gridObject;
    private PerspectiveController perspectiveController;
    public float alphaMultiplier_perspectiveMode = 0.1f;

    /// <summary>
    /// 判断是否遮挡其他地面物体
    /// </summary>
    public bool CoverCheck()
    {
        Vector3Int p = gridObject.CellPosition + IsometricGridManager.CoverVector;
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
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (CoverCheck())
                SetAlpha(spriteRenderers[i], alphaMultiplier_perspectiveMode * alphas[i]);
        }
    }

    public void ExitPerspectiveMode()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            SetAlpha(spriteRenderers[i], alphas[i]);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        igm = IsometricGridManager.FindInstance();
        perspectiveController = igm.PerspectiveController;
        gridObject = GetComponent<GridObject>();
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
