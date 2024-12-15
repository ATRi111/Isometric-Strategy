using EditorExtend.GridEditor;
using UnityEngine;

public class GroundPerspectiveController : SpriteController
{
    private GridObject gridObject;
    private PerspectiveController perspectiveController;
    public float alphaMultiplier_perspectiveMode = 0.1f;

    public void EnterPerspectiveMode()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (perspectiveController.CoverCheck(gridObject.CellPosition))
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
        perspectiveController = IsometricGridManager.FindInstance().PerspectiveController;
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
