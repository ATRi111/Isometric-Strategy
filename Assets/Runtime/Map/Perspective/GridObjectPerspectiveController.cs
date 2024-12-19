using EditorExtend.GridEditor;

public class GridObjectPerspectiveController : PerspectiveController
{
    private GridObject gridObject;

    protected override bool CoverCheck()
    {
        return perspectiveController.CoverCheck(gridObject.CellPosition);
    }

    protected override void ExitPerspectiveMode()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            SetAlpha(spriteRenderers[i], alphas[i]);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        gridObject = GetComponent<GridObject>();
    }
}
