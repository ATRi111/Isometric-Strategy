using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class IsometricObjectBrush : ObjectBrush
    {
#if UNITY_EDITOR
        public bool pillarMode;
        public bool lockLayer;
        public int layer;
        public bool lockXY;

        public override Vector3Int CalculateCellPosition(Vector3 worldPosition, Vector3Int lockedPosition)
        {
            IsometricGridManagerBase igm = Manager as IsometricGridManagerBase;
            if(lockXY)
            {
                cellPosition = igm.ClosestZ(lockedPosition.ResetZ(cellPosition.z), worldPosition);  //锁定后，X和Y不能变化
                return cellPosition;
            }

            if (lockLayer)
            {
                float z = igm.LayerToWorldZ(layer);
                Vector3Int temp = Manager.WorldToCell(worldPosition.ResetZ(z));
                int deltaX = Mathf.Abs(temp.x - lockedPosition.x);
                int deltaY = Mathf.Abs(temp.y - lockedPosition.y);
                if(deltaX > 0 && deltaY != 0)       //锁定后，X和Y只能有一个变化
                {
                    if(deltaX >= deltaY)
                        temp.y = lockedPosition.y;
                    else
                        temp.x = lockedPosition.x;
                }
                return temp;
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