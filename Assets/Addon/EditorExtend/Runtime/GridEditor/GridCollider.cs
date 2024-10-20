using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    [RequireComponent(typeof(GridObject))]
    public abstract class GridCollider : MonoBehaviour,IGridShape
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
        protected Vector3Int CellPosition => GridObject.CellPosition;

        public abstract void GetStrip(List<Vector3> ret);

        public abstract bool Overlap(Vector3 p);

        protected virtual void Awake()
        {
            gridObject.OverlapFunc = Overlap;
        }
    }
}