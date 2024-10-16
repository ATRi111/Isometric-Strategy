using UnityEngine;

namespace EditorExtend.GridEditor
{
    [SelectionBase]
    [RequireComponent(typeof(Grid))]
    public abstract class GridManager : MonoBehaviour
    {
        private Grid grid;
        public Grid Grid
        {
            get
            {
#if UNITY_EDITOR
                if (grid == null)
                    grid = GetComponent<Grid>();
#endif
                return grid;
            }
        }

        public abstract int CellToSortingOrder(Vector3Int cell);
    }
}