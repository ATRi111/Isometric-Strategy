using System;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class GridObject : MonoBehaviour
    {
        #region ���
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

        #region λ��
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
        /// cellPosition��Z����,ȷ��XY��ʹ��Ӧ������������ӽ���ǰ��������
        /// </summary>
        public Vector3Int AlignXY()
        {
            cellPosition = Manager.ClosestCell(transform.position);
            Refresh(cellPosition);
            return cellPosition;
        }
        /// <summary>
        /// cellPosition��XY����,ȷ��һ��Z��ʹ��Ӧ������������ӽ���ǰ��������
        /// </summary>
        public Vector3Int AlignZ()
        {
            cellPosition = Manager.ClosestZ(cellPosition, transform.position);
            Refresh(cellPosition);
            return cellPosition;
        }
        #endregion

        #region ��Ϸ�߼�
        public Func<int> ObstacleHeightFunc;
        /// <summary>
        /// �����ϰ�������ʱ��������ĸ߶�
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
        /// ����ռ�ݵķ�Χ�Ƿ񸲸����������µ�ĳ��
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