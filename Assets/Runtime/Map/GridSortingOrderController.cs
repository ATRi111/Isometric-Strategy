using EditorExtend.GridEditor;

public class GridSortingOrderController : GridSortingOrderControllerBase
{
    protected override GridManagerBase Manager
    {
        get
        {
            if (manager == null)
                manager = IsometricGridManager.Instance;
            return manager;
        }
    }

    protected virtual void Update()
    {
        spriteRenderer.sortingOrder = Manager.CellToSortingOrder(transform.position) + extraSortingOrder;
    }
}