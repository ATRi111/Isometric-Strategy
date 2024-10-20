using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class IsometricGridManagerBase : GridManagerBase
    {
        //Isometric地图在同一(x,y)处可能有多个层数不同的物体，使用两个字典记录某位置的层数范围，以提高查询效率
        private readonly Dictionary<Vector2Int, int> maxLayerDict = new();
        public int maxLayer;

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
            float cellZf = deltaWorldY / Grid.cellSize.y / Grid.cellSize.z * 2f;
            int cellZ = Mathf.Max(0, Mathf.RoundToInt(cellZf));  //0层以下禁止绘制
            return xy.ResetZ(cellZ);
        }

        /// <summary>
        /// 根据世界坐标（忽略z）确定一些列网格坐标，判断这些网格坐标上是否有物体，若有则返回其中的最高层数
        /// </summary>
        public bool MatchMaxLayer(Vector3 worldPosition, out int layer)
        {
            for (int currentLayer = maxLayer; currentLayer >= 0; currentLayer--)
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
            maxLayer = 0;
            maxLayerDict.Clear();
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
            }
            else
            {
                maxLayerDict[xy] = Mathf.Max(maxLayerDict[xy], layer);
            }
            maxLayer = Mathf.Max(maxLayer, layer);
        }

        /// <summary>
        /// 获取xy坐标上的所有物体
        /// </summary>
        public void GetObejectsXY(Vector2Int xy, List<GridObject> objects)
        {
            objects.Clear();
            if (!maxLayerDict.ContainsKey(xy))
                return;
            for (int layer = maxLayerDict[xy]; layer >= 0; layer--) 
            {
                Vector3Int temp = xy.AddZ(layer);
                GridObject obj = GetObject(temp);
                if (obj != null)
                    objects.Add(obj);
            }
        }
        /// <summary>
        /// 获取xy坐标上层数最高的物体
        /// </summary>
        public GridObject GetObejectXY(Vector2Int xy)
        {
            if (!maxLayerDict.ContainsKey(xy))
                return null;
            for (int layer = maxLayerDict[xy]; layer >= 0; layer--)
            {
                Vector3Int temp = xy.AddZ(layer);
                GridObject obj = GetObject(temp);
                if (obj != null)
                    return obj;
            }
            return null;
        }
    }
}