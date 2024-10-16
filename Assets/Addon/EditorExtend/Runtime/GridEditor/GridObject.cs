using EditorExtend.GridEditor;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    private GridManager manager;

    public GridManager Manager
    {
        get
        {
#if UNITY_EDITOR
            if (manager == null)
                manager = GetComponentInParent<GridManager>();
#endif
            return manager;
        }
    }

    private SpriteRenderer spriteRenderer;

    public SpriteRenderer SpriteRenderer
    {
        get
        {
#if UNITY_EDITOR
            if (spriteRenderer == null)
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
#endif
            return spriteRenderer;
        }
    }

    [SerializeField]
    private Vector3Int cellPosition;
    public Vector3Int CellPosition
    {
        get => cellPosition;
        set
        {
            cellPosition = value;
            Refresh(value);
        }
    }

    public void Refresh(Vector3Int cell)
    {
        transform.position = Manager.Grid.CellToWorld(cell);
        SpriteRenderer.sortingOrder = Manager.CellToSortingOrder(cell);
    }
}
