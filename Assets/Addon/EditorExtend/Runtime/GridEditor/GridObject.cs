using System;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class GridObject : MonoBehaviour
    {
        #region 组件
        private GridManagerBase manager;
        public GridManagerBase Manager
        {
            get
            {
                if (manager == null)
                    manager = GetComponentInParent<GridManagerBase>();
                return manager;
            }
        }

        private SpriteRenderer spriteRenderer;
        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (spriteRenderer == null)
                    spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                return spriteRenderer;
            }
        }

        [SerializeField]
        private string shortName;
        public string ShortName
        {
            get
            {
                if (string.IsNullOrEmpty(shortName))
                    shortName = ExternalTool.GetShortName(gameObject);
                return shortName;
            }
        }
        #endregion

        #region 位置
        [SerializeField]
        protected Vector3Int cellPosition;
        public Vector3Int CellPosition
        {
            get => cellPosition;
            set
            {
                cellPosition = value;
                Refresh(value);
            }
        }

        public Vector3 Refresh()
            => Refresh(cellPosition);
        public Vector3 Refresh(Vector3Int cell)
        {
            transform.position = Manager.Grid.CellToWorld(cell);
            SpriteRenderer.sortingOrder = Manager.CellToSortingOrder(cell);
            return transform.position;
        }
        /// <summary>
        /// cellPosition的Z不变,确定XY，使对应的世界坐标最接近当前世界坐标
        /// </summary>
        public Vector3Int AlignXY()
        {
            cellPosition = Manager.ClosestCell(transform.position);
            Refresh(cellPosition);
            return cellPosition;
        }
        /// <summary>
        /// cellPosition的XY不变,确定一个Z，使对应的世界坐标最接近当前世界坐标
        /// </summary>
        public Vector3Int AlignZ()
        {
            cellPosition = Manager.ClosestZ(cellPosition, transform.position);
            Refresh(cellPosition);
            return cellPosition;
        }
        #endregion

        #region 游戏逻辑
        public Func<int> ObstacleHeightFunc;
        /// <summary>
        /// 发挥障碍物作用时，此物体的高度
        /// </summary>
        public virtual int ObstacleHeight
        {
            get
            {
                if (ObstacleHeightFunc != null)
                    return ObstacleHeightFunc.Invoke();
                return 1;
            }
        }

        public Func<Vector3, bool> OverlapFunc;
        /// <summary>
        /// 物体占据的范围是否覆盖网格坐标下的某点
        /// </summary>
        public virtual bool Overlap(Vector3 p)
        {
            if(OverlapFunc != null)
                return OverlapFunc.Invoke(p);
            return GridUtility.BoxOverlap(cellPosition, Vector3Int.one, p);
        }

        #endregion
    }
}