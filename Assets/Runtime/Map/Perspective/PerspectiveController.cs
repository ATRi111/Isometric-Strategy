using UIExtend;
using UnityEngine;

public abstract class PerspectiveController : AlphaController
{
    protected PerspectiveManager perspectiveManager;
    [SerializeField]
    protected CanvasGroupPlus[] canvasGroups;
    public float alphaMultiplier_perspectiveMode = 0.1f;

    protected abstract bool CoverCheck();

    protected virtual void EnterPerspectiveMode()
    {
        if (CoverCheck())
        {
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                SetAlpha(spriteRenderers[i], alphaMultiplier_perspectiveMode * alphas[i]);
            }
            for (int i = 0;i < canvasGroups.Length; i++)
            {
                canvasGroups[i].Visible = false;
            }
        }
    }
    protected virtual void ExitPerspectiveMode()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            SetAlpha(spriteRenderers[i], alphas[i]);
        }
        for (int i = 0; i < canvasGroups.Length; i++)
        {
            canvasGroups[i].Visible = true;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        perspectiveManager = PerspectiveManager.FindInstance();
    }

    protected virtual void OnEnable()
    {
        perspectiveManager.EnterPerspectiveMode += EnterPerspectiveMode;
        perspectiveManager.ExitPerspectiveMode += ExitPerspectiveMode;
    }

    protected virtual void OnDisable()
    {
        perspectiveManager.EnterPerspectiveMode -= EnterPerspectiveMode;
        perspectiveManager.ExitPerspectiveMode -= ExitPerspectiveMode;
    }
}
