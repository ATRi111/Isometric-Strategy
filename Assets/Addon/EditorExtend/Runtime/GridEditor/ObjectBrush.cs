using UnityEngine;

namespace EditorExtend.GridEditor
{
    [RequireComponent(typeof(GridManagerBase))]
    public abstract class ObjectBrush : MonoBehaviour
    {
#if UNITY_EDITOR
        private GridManagerBase manager;
        public GridManagerBase Manager
        {
            get
            {
                if (manager == null)
                    manager = GetComponentInParent<GridManagerBase>();
                return manager;
            }
        }

        public GameObject prefab;
        public Vector3Int cellPosition;

        public abstract Vector3Int CalculateCellPosition(Vector3 worldPosition);
#endif
    }
}