using UnityEngine;

public class CanvasSortingOrderController : MonoBehaviour
{
    [Range(0, 50)]
    public int extraSortingOrder;

    private IsometricGridManager Igm => IsometricGridManager.Instance;

    private Canvas canvas;
    public Canvas Canvas
    {
        get
        {
            if (canvas == null)
                canvas = GetComponent<Canvas>();
            return canvas;
        }
    }
    protected virtual void Update()
    {
        if (Igm != null)
            Canvas.sortingOrder = Igm.CellToSortingOrder(transform.position) + extraSortingOrder;
    }

    private void OnDrawGizmos()
    {
        if (Igm != null)
            Canvas.sortingOrder = Igm.CellToSortingOrder(transform.position) + extraSortingOrder;
    }
}