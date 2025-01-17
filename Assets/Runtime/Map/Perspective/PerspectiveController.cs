using UIExtend;

public abstract class PerspectiveController : AlphaController
{
    protected CanvasGroupPlus canvasGroup;
    protected PerspectiveManager perspectiveManager;
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
            if (canvasGroup != null)
                canvasGroup.Visible = false;
        }
    }
    protected virtual void ExitPerspectiveMode()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            SetAlpha(spriteRenderers[i], alphas[i]);
        }
        if (canvasGroup != null)
            canvasGroup.Visible = true;
    }

    protected override void Awake()
    {
        base.Awake();
        perspectiveManager = PerspectiveManager.FindInstance();
        canvasGroup = GetComponentInChildren<CanvasGroupPlus>();
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
