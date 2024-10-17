using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class IsometricGridManager : GridManager
    {
        //Isometric地图在同一(x,y)处可能有多个层数不同的物体，使用两个字典记录某位置的层数范围，以提高查询效率
        private readonly Dictionary<Vector2Int, int> maxLayerDict = new();
        private readonly Dictionary<Vector2Int, int> minLayerDict = new();
        public int maxLayer;
        public int minLayer;

        public override int CellToSortingOrder(Vector3Int cell)
        {
            return -cell.x - cell.y + cell.z;
        }

        public float LayerToWorldZ(int layer)
        {
            return layer * Grid.cellSize.z;
        }

        protected override void AddObject(GridObject gridObject)
        {
            base.AddObject(gridObject);
            Vector2Int xy = new(gridObject.CellPosition.x, gridObject.CellPosition.y);
            int layer = gridObject.CellPosition.z;
            if (!maxLayerDict.ContainsKey(xy))
            {
                maxLayerDict.Add(xy, layer);
                minLayerDict.Add(xy, layer);
            }
            else
            {
                maxLayerDict[xy] = Mathf.Max(maxLayerDict[xy], layer);
                minLayerDict[xy] = Mathf.Min(minLayerDict[xy], layer);
            }
            maxLayer = Mathf.Max(maxLayer, layer);
            minLayer = Mathf.Min(minLayer, layer);
        }
    }
}