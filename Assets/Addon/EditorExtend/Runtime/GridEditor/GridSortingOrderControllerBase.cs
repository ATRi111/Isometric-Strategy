using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class GridSortingOrderControllerBase : MonoBehaviour
    {
        public static void RefreshChildren(Component obj)
        {
            if (obj != null)
            {
                GridSortingOrderControllerBase[] controllers = obj.GetComponentsInChildren<GridSortingOrderControllerBase>();
                for (int k = 0; k < controllers.Length; k++)
                {
                    controllers[k].RefreshSortingOrder();
                }
            }
        }

        protected GridManagerBase manager;
        protected SpriteRenderer spriteRenderer;
        [Range(-10, 30)]
        public int extraSortingOrder;

        protected virtual GridManagerBase GetGridManager()
        {
            return GetComponentInParent<GridManagerBase>();
        }

        protected virtual void Start()
        {
            RefreshSortingOrder();
        }

        public void RefreshSortingOrder()
        {
            manager = GetGridManager();
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = manager.CellToSortingOrder(transform.position) + extraSortingOrder;
        }
    }
}