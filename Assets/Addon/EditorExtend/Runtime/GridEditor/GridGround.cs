using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class GridGround : MonoBehaviour
    {
        private GridObject gridObject;
        protected GridObject GridObject
        {
            get
            {
                if (gridObject == null)
                    gridObject = GetComponent<GridObject>();
                return gridObject;
            }
        }

        public int height;
        public virtual int GroundHeight()
            => height;

        protected virtual void Awake()
        {
            GridObject.GroundHeightFunc = GroundHeight;
        }

    }
}