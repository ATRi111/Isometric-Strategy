using UnityEngine;

namespace EditorExtend.GridEditor
{
    [RequireComponent(typeof(GridManager))]
    public class ObjectBrush : MonoBehaviour
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

        public Vector3Int cellPosition;

        public bool lockZ = true;
        public int lockedZ;

        public Vector3Int CalculateCellPosition(Vector3 worldPosition)
        {
            int z = lockedZ;
            Vector3 ZtoY = Manager.Grid.CellToWorld(z * Vector3Int.forward);
            Vector3 world = new Vector3(worldPosition.x, worldPosition.y, 0f);  //Z²»Ó°ÏìÍ¼ÏñÎ»ÖÃ
            return Manager.Grid.WorldToCell(world) + z * Vector3Int.forward;
        }
#endif
    }
}