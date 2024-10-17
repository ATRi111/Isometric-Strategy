using UnityEngine;

namespace EditorExtend.GridEditor
{
    [RequireComponent(typeof(GridManager))]
    public abstract class ObjectBrush : MonoBehaviour
    {
#if UNITY_EDITOR
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

        public GameObject prefab;
        public Vector3Int cellPosition;

        public abstract Vector3Int CalculateCellPosition(Vector3 worldPosition);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 offset = Manager.CellToWorld(new Vector3Int(1, 1, 0));
            Gizmos.DrawSphere(Manager.CellToWorld(cellPosition) + offset, 0.05f);
        }
#endif
    }
}