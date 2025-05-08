using EditorExtend.GridEditor;

public class GridSortingOrderController : GridSortingOrderControllerBase
{
    protected override GridManagerBase GetGridManager()
    {
        return IsometricGridManager.Instance;
    }

    protected virtual void Update()
    {
        spriteRenderer.sortingOrder = manager.CellToSortingOrder(transform.position) + extraSortingOrder;
    }
}