using System;
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
                return cellPosition;
            }

            if (lockLayer)
            {
                float z = igm.LayerToWorldZ(layer);  //世界坐标的z分量不影响图像位置,但影响转换后cellPosition的z及xy
                return Manager.WorldToCell(worldPosition.ResetZ(z));
            }

            return igm.AbsorbSurfaceOfCube(worldPosition);
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
        }
#endif
    }
}