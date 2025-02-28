using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class StaticGridSortingOrderController : MonoBehaviour
    {
        [Range(0, 30)]
        public int extraSortingOrder;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            spriteRenderer.sortingOrder = IsometricGridManager.Instance.CellToSortingOrder(transform.position) + extraSortingOrder;
        }

        public void Refresh()
        {

        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (spriteRenderer == null && !Application.isPlaying)
            {
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                spriteRenderer.sortingOrder = IsometricGridManager.Instance.CellToSortingOrder(transform.position) + extraSortingOrder;
            }
        }
#endif
    }
}