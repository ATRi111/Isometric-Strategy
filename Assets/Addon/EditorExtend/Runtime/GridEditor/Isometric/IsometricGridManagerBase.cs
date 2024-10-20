using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class IsometricGridManagerBase : GridManagerBase
    {
        //Isometric地图在同一(x,y)处可能有多个层数不同的物体，使用两个字典记录某位置的层数范围，以提高查询效率
        private readonly Dictionary<Vector2Int, int> maxLayerDict = new();
        private readonly Dictionary<Vector2Int, int> minLayerDict = new();
        public int maxLayer;
        public int minLayer;

        public override Vector3 CellToWorld(Vector3 cellPosition)
        {
            Vector3 local = IsometricGridUtility.CellToWorld(cellPosition, Grid.cellSize);
            return transform.TransformPoint(local);
        }

        public override int CellToSortingOrder(Vector3Int cell)
        {
            return -cell.x - cell.y + cell.z;
        }

        public float LayerToWorldZ(int layer)
        {
            return layer * Grid.cellSize.z;
        }

        public override void AddObject(GridObject gridObject)
        {
            base.AddObject(gridObject);
            Vector2Int xy = (Vector2Int)gridObject.CellPosition;
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

        public void GetObejectsXY(Vector2Int xy, List<GridObject> objects)
        {
            objects.Clear();
            if (!maxLayerDict.ContainsKey(xy))
                return;
            GridObject obj;
            Vector3Int cellPosition;
            for (int layer = minLayerDict[xy]; layer <= maxLayerDict[xy]; layer++) 
            {
                cellPosition = xy.AddZ(layer);
                obj = this[cellPosition];
                objects.Add(obj);
            }
        }
    }
}