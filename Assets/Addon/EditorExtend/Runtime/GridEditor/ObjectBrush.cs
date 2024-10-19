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
#endif
    }
}