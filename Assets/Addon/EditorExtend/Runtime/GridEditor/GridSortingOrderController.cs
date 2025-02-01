using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class GridSortingOrderController : MonoBehaviour
    {
        [Range(0, 20)]
        public int extraSortingOrder;

        private IsometricGridManager Igm => IsometricGridManager.Instance;

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
            if(Igm != null)
                SpriteRenderer.sortingOrder = Igm.CellToSortingOrder(transform.position) + extraSortingOrder;
        }

        private void OnDrawGizmos()
        {
            if (Igm != null)
                SpriteRenderer.sortingOrder = Igm.CellToSortingOrder(transform.position) + extraSortingOrder;
        }
    }
}