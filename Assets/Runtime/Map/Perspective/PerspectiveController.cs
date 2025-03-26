public abstract class PerspectiveController : AlphaController
{
    protected PerspectiveManager perspectiveManager;
    public float alphaMultiplier_perspectiveMode = 0.1f;
    private bool perspectived;

    protected abstract bool CoverCheck();

    protected virtual void EnterPerspectiveMode()
    {
        if (CoverCheck() && !perspectived)
        {
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                SetAlpha(spriteRenderers[i], alphaMultiplier_perspectiveMode);
            }
            for (int i = 0;i < canvasGroups.Length; i++)
            {
                SetAlpha(canvasGroups[i], alphaMultiplier_perspectiveMode);
            }
            perspectived = true;
        }
    }
    protected virtual void ExitPerspectiveMode()
    {
        if(perspectived)
        {
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                SetAlpha(spriteRenderers[i], 1 / alphaMultiplier_perspectiveMode);
            }
            for (int i = 0; i < canvasGroups.Length; i++)
            {
                SetAlpha(canvasGroups[i], 1 / alphaMultiplier_perspectiveMode);
            }
            perspectived = false;
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
        perspectived = false;
    }

    protected virtual void OnDisable()
    {
        perspectiveManager.EnterPerspectiveMode -= EnterPerspectiveMode;
        perspectiveManager.ExitPerspectiveMode -= ExitPerspectiveMode;
    }
}
