using EditorExtend.GridEditor;

public class GridObjectPerspectiveController : PerspectiveController
{
    private GridObject gridObject;

    protected override bool CoverCheck()
    {
        return perspectiveManager.CoverCheck(gridObject.CellPosition);
    }

    protected override void Awake()
    {
        base.Awake();
        gridObject = GetComponent<GridObject>();
    }
}
