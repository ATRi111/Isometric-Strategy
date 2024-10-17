using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class IsometricObjectBrush : ObjectBrush
    {
#if UNITY_EDITOR
        public bool lockLayer = true;
        public int lockedLayer;
        public override Vector3Int CalculateCellPosition(Vector3 worldPosition)
        {
            IsometricGridManager igm = Manager as IsometricGridManager;
            if (lockLayer)
            {
                float zOffset = igm.LayerToWorldZ(lockedLayer);  //世界坐标的z分量不影响图像位置,但影响转换后cellPosition的z及xy
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
#endif
    }
}