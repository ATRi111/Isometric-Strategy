using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class GridSortingOrderController : MonoBehaviour
    {
        public int extraSortingOrder;

        private GridManagerBase manager;
        public GridManagerBase Manager
        {
            get
            {
                if (manager == null)
                    manager = GridManagerBase.FindInstance?.Invoke();
                return manager;
            }
        }

        private SpriteRenderer spriteRenderer;
        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (spriteRenderer == null)
                    spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                return spriteRenderer;
            }
        }
        protected virtual void Update()
        {
            SpriteRenderer.sortingOrder = Manager.CellToSortingOrder(transform.position) + extraSortingOrder;
        }

        private void OnDrawGizmos()
        {
            if (Manager != null)
                SpriteRenderer.sortingOrder = Manager.CellToSortingOrder(transform.position) + extraSortingOrder;
        }
    }
}