using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class GridObject : MonoBehaviour
    {
        private GridManager manager;

        public GridManager Manager
        {
            get
            {
                if (manager == null)
                    manager = GetComponentInParent<GridManager>();
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

        public Vector3 Refresh()
            => Refresh(cellPosition);
        public Vector3 Refresh(Vector3Int cell)
        {
            transform.position = Manager.Grid.CellToWorld(cell);
            SpriteRenderer.sortingOrder = Manager.CellToSortingOrder(cell);
            return transform.position;
        }
        public Vector3Int Align()
        {
            cellPosition = Manager.ClosestCell(transform.position);
            Refresh(cellPosition);
            return cellPosition;
        }
    }
}