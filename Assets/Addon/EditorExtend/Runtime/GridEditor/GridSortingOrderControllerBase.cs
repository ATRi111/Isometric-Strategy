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

        [Range(-10, 30)]
        public int extraSortingOrder;

        protected virtual void Start()
        {
            RefreshSortingOrder();
        }

        public void RefreshSortingOrder()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = IsometricGridManager.Instance.CellToSortingOrder(transform.position) + extraSortingOrder;
        }
    }
}