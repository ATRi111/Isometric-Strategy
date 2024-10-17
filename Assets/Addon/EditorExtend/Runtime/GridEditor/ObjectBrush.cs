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

        private static Vector3Int[] CellPositions =
        {
            Vector3Int.zero,
            Vector3Int.up,
            Vector3Int.up + Vector3Int.forward,
            Vector3Int.forward,
            Vector3Int.zero,
            Vector3Int.right,
            Vector3Int.right + Vector3Int.forward,
            Vector3Int.forward,
            Vector3Int.up + Vector3Int.forward,
            Vector3Int.one,
            Vector3Int.right + Vector3Int.forward,
        };
        private static Vector3Int[] BottomCellPositions =
        {
            Vector3Int.up,
            Vector3Int.zero,
            Vector3Int.right,
        };

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3[] points = new Vector3[CellPositions.Length];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = Manager.CellToWorld(CellPositions[i] + cellPosition);
            }
            Gizmos.DrawLineStrip(points, false);
            if(cellPosition.z > 0)
            {
                Gizmos.color = Color.green;
                points = new Vector3[BottomCellPositions.Length * 2];
                for (int i = 0; i < BottomCellPositions.Length; i++)
                {
                    points[i * 2] = Manager.CellToWorld(BottomCellPositions[i] + cellPosition);
                    points[i * 2 + 1] = Manager.CellToWorld(BottomCellPositions[i] + cellPosition - cellPosition.z * Vector3Int.forward);
                }
                Gizmos.DrawLineList(points);
            }
            if(cellPosition.z < - 1)
            {
                Gizmos.color = Color.blue;
                points = new Vector3[BottomCellPositions.Length * 2];
                for (int i = 0; i < BottomCellPositions.Length; i++)
                {
                    points[i * 2] = Manager.CellToWorld(BottomCellPositions[i] + cellPosition + Vector3Int.forward);
                    points[i * 2 + 1] = Manager.CellToWorld(BottomCellPositions[i] + cellPosition - cellPosition.z * Vector3Int.forward);
                }
                Gizmos.DrawLineList(points);
            }
        }
#endif
    }
}