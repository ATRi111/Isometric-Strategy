using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class IsometricObjectBrush : ObjectBrush
    {
#if UNITY_EDITOR
        public bool lockLayer;
        public int layer;
        public bool lockXY;

        public override Vector3Int CalculateCellPosition(Vector3 worldPosition)
        {
            IsometricGridManagerBase igm = Manager as IsometricGridManagerBase;
            if(lockXY)
            {
                return igm.ClosestZ(cellPosition, worldPosition);
            }
            
            if (lockLayer)
            {
                float zOffset = igm.LayerToWorldZ(layer);  //世界坐标的z分量不影响图像位置,但影响转换后cellPosition的z及xy
                worldPosition = new(worldPosition.x, worldPosition.y, transform.position.z + zOffset);
                return Manager.WorldToCell(worldPosition);
            }

            GridObject gridObject;
            for (int layer = igm.maxLayer; layer >= igm.minLayer; layer--)
            {
                float z = igm.LayerToWorldZ(layer);
                worldPosition = new Vector3(worldPosition.x, worldPosition.y, z);
                cellPosition = Manager.WorldToCell(worldPosition);
                gridObject = igm[cellPosition];
                if (gridObject != null)
                    return gridObject.CellPosition;
            }
            worldPosition = new Vector3(worldPosition.x, worldPosition.y, 0f);
            return Manager.WorldToCell(worldPosition);
        }

        private static readonly Vector3Int[] CellPositions =
        {
            Vector3Int.zero,
            Vector3Int.up,
            Vector3Int.up + Vector3Int.forward,
            Vector3Int.forward,
            Vector3Int.zero,
            Vector3Int.right,
            Vector3Int.right + Vector3Int.forward,
            Vector3Int.forward,
            Vector3Int.up + Vector3Int.forward,
            Vector3Int.one,
            Vector3Int.right + Vector3Int.forward,
        };
        private static readonly Vector3Int[] BottomCellPositions =
        {
            Vector3Int.up,
            Vector3Int.zero,
            Vector3Int.right,
        };
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3[] points = new Vector3[CellPositions.Length];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = Manager.CellToWorld((Vector3)(CellPositions[i] + cellPosition));
            }
            Gizmos.DrawLineStrip(points, false);
            if (cellPosition.z > 0)
            {
                Gizmos.color = Color.green;
                points = new Vector3[BottomCellPositions.Length * 2];
                for (int i = 0; i < BottomCellPositions.Length; i++)
                {
                    points[i * 2] = Manager.CellToWorld(BottomCellPositions[i] + cellPosition);
                    points[i * 2 + 1] = Manager.CellToWorld(BottomCellPositions[i] + cellPosition - cellPosition.z * Vector3Int.forward);
                }
                Gizmos.DrawLineList(points);
            }
            if (cellPosition.z < -1)
            {
                Gizmos.color = Color.blue;
                points = new Vector3[BottomCellPositions.Length * 2];
                for (int i = 0; i < BottomCellPositions.Length; i++)
                {
                    points[i * 2] = Manager.CellToWorld(BottomCellPositions[i] + cellPosition + Vector3Int.forward);
                    points[i * 2 + 1] = Manager.CellToWorld(BottomCellPositions[i] + cellPosition - cellPosition.z * Vector3Int.forward);
                }
                Gizmos.DrawLineList(points);
            }
        }
#endif
    }
}