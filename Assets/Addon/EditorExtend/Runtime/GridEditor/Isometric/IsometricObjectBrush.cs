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
                cellPosition = igm.ClosestZ(cellPosition, worldPosition);
            }
            else if (lockLayer)
            {
                float zOffset = igm.LayerToWorldZ(layer);  //世界坐标的z分量不影响图像位置,但影响转换后cellPosition的z及xy
                worldPosition = new(worldPosition.x, worldPosition.y, transform.position.z + zOffset);
                cellPosition = Manager.WorldToCell(worldPosition);
            }
            else
            {
                bool match = false;
                GridObject gridObject;
                for (int layer = igm.maxLayer; layer >= igm.minLayer; layer--)
                {
                    float z = igm.LayerToWorldZ(layer);
                    worldPosition = worldPosition.ResetZ(z);
                    cellPosition = Manager.WorldToCell(worldPosition);
                    gridObject = igm[cellPosition];
                    if (gridObject != null)
                    {
                        match = true;
                        cellPosition = gridObject.CellPosition;
                    }
                }
                if (!match)
                {
                    worldPosition = worldPosition.ResetZ();
                    cellPosition = Manager.WorldToCell(worldPosition);
                }
            }
            return cellPosition;
        }

        private static readonly Vector3Int[] BottomCellPositions =
        {
            Vector3Int.up,
            Vector3Int.zero,
            Vector3Int.right,
        };
        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Vector3[] points;
            if (cellPosition.z > 0)
            {
                Gizmos.color = Color.green;
                points = new Vector3[BottomCellPositions.Length * 2];
                for (int i = 0; i < BottomCellPositions.Length; i++)
                {
                    points[i * 2] = Manager.CellToWorld(BottomCellPositions[i] + cellPosition).ResetZ();
                    points[i * 2 + 1] = Manager.CellToWorld(BottomCellPositions[i] + cellPosition - cellPosition.z * Vector3Int.forward).ResetZ();
                }
                Gizmos.DrawLineList(points);
            }
            if (cellPosition.z < -1)
            {
                Gizmos.color = Color.blue;
                points = new Vector3[BottomCellPositions.Length * 2];
                for (int i = 0; i < BottomCellPositions.Length; i++)
                {
                    points[i * 2] = Manager.CellToWorld(BottomCellPositions[i] + cellPosition).ResetZ();
                    points[i * 2 + 1] = Manager.CellToWorld(BottomCellPositions[i] + cellPosition - cellPosition.z * Vector3Int.forward).ResetZ();
                }
                Gizmos.DrawLineList(points);
            }
        }
#endif
    }
}