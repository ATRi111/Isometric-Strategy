using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    //此脚本所在的游戏物体不可平移/缩放/旋转
    [SelectionBase]
    [RequireComponent(typeof(Grid))]
    public abstract class GridManagerBase : MonoBehaviour
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
        public Dictionary<Vector3Int, GridObject> ObjectDict
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

        public GridObject this[Vector3Int cellPosition]
        {
            get => GetObject(cellPosition);
            set
            {
                RemoveObject(cellPosition);
                if(value != null)
                    AddObject(value);
            }
        }

        public Vector2 centerOffset = new(0.33f, 0.33f);

        public virtual Vector3 CellToWorld(Vector3Int cellPosition)
            => Grid.CellToWorld(cellPosition);
        public virtual Vector3 CellToWorld(Vector3 cellPosition)
            => CellToWorld(cellPosition.Integerized());
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
        /// 给定xy,确定一个z,使cellPositoin最接近worldPosition
        /// </summary>
        public virtual Vector3Int ClosestZ(Vector3Int xy, Vector3 worldPosition)
        {
            return xy;
        }

        /// <summary>
        /// 根据CellPosition自动计算SortingOrder
        /// </summary>
        public abstract int CellToSortingOrder(Vector3Int cell);

        public virtual void AddAllObjects()
        {
            ObjectDict.Clear();
            GridObject[] objects = GetComponentsInChildren<GridObject>();
            for (int i = 0; i < objects.Length; i++)
            {
                AddObject(objects[i]);
            }
        }

        public virtual GridObject GetObject(Vector3Int cellPosition)
        {
            if (ObjectDict.TryGetValue(cellPosition, out GridObject ret))
                return ret;
            return null;
        }

        public virtual void AddObject(GridObject gridObject)
        {
            if(!ObjectDict.ContainsKey(gridObject.CellPosition))
                ObjectDict.Add(gridObject.CellPosition, gridObject);
        }

        public virtual void RemoveObject(Vector3Int cellPosition)
        {
            if (ObjectDict.ContainsKey(cellPosition))
            {
                GridObject gridObject = ObjectDict[cellPosition];
                ObjectDict.Remove(cellPosition);
                ExternalTool.Destroy(gridObject.gameObject);
            }
        }

        public virtual bool CanPlaceAt(Vector3Int cellPosition)
        {
            return !ObjectDict.ContainsKey(cellPosition);
        }
    }
}