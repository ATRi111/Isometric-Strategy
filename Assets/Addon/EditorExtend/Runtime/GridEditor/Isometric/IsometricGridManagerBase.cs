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
            => IsometricGridUtility.CellToWorld(cellPosition, Grid.cellSize);

        public override int CellToSortingOrder(Vector3Int cell)
        {
            return -cell.x - cell.y + cell.z;
        }

        public float LayerToWorldZ(int layer)
        {
            return layer * Grid.cellSize.z;
        }

        public override Vector3Int ClosestZ(Vector3Int xy, Vector3 worldPosition)
        {
            xy = xy.ResetZ();
            Vector3 worldBase = CellToWorld(xy);
            float deltaWorldY = worldPosition.y - worldBase.y;
            float deltaCellZ = deltaWorldY / Grid.cellSize.y / Grid.cellSize.z * 2f;
            return xy + Mathf.FloorToInt(deltaCellZ) * Vector3Int.forward;
        }

        /// <summary>
        /// 根据世界坐标（忽略z）确定一些列网格坐标，判断这些网格坐标上是否有物体，若有则返回其中的最高层数
        /// </summary>
        public bool MatchMaxLayer(Vector3 worldPosition, out int layer)
        {
            for (int currentLayer = maxLayer; currentLayer >= minLayer; currentLayer--)
            {
                float z = LayerToWorldZ(currentLayer);
                worldPosition = worldPosition.ResetZ(z);
                Vector3Int temp = WorldToCell(worldPosition);
                if (GetObject(temp) != null)
                {
                    layer = currentLayer;
                    return true;
                }
            }
            layer = 0;
            return false;
        }

        public override void AddAllObjects()
        {
            minLayer = maxLayer = 0;
            maxLayerDict.Clear();
            minLayerDict.Clear();
            base.AddAllObjects();
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