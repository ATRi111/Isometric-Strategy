using System.Collections.Generic;
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
                if (grid == null)
                    grid = GetComponent<Grid>();
                return grid;
            }
        }

        private Dictionary<Vector3Int, GridObject> objectDict;
        private Dictionary<Vector3Int, GridObject> ObjectsDict
        {
            get
            {
                if (objectDict == null)
                {
                    objectDict = new Dictionary<Vector3Int, GridObject>();
                    AddAllObjects();
                }
                return objectDict;
            }
        }

        public Vector2 centerOffset = new(0.33f, 0.33f);

        public virtual Vector3 CellToWorld(Vector3Int cellPosition)
            => Grid.CellToWorld(cellPosition);
        public virtual Vector3Int WorldToCell(Vector3 worldPosition)
           => Grid.WorldToCell(worldPosition);
        public virtual Vector3Int ClosestCell(Vector3 worldPosition)
        {
            Vector3 offset = CellToWorld(new Vector3Int(1, 1, 0));
            offset = new Vector3(offset.x * centerOffset.x, offset.y * centerOffset.y, 0f);
            worldPosition += offset;
            return WorldToCell(worldPosition);
        }

        /// <summary>
        /// 根据CellPosition自动计算SortingOrder
        /// </summary>
        public abstract int CellToSortingOrder(Vector3Int cell);

        public void AddAllObjects()
        {
            objectDict.Clear();
            GridObject[] objects = GetComponentsInChildren<GridObject>();
            for (int i = 0; i < objects.Length; i++)
            {
                AddObject(objects[i]);
            }
        }

        protected virtual void AddObject(GridObject gridObject)
        {
            objectDict.Add(gridObject.CellPosition, gridObject);
        }

        public virtual GridObject GetObject(Vector3Int cellPosition)
        {
            if (ObjectsDict.TryGetValue(cellPosition, out GridObject ret))
                return ret;
            return null;
        }
    }
}