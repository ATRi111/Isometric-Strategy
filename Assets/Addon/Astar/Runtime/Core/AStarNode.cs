using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public class AStarNode
    {
        protected readonly PathFindingProcess process;

        public Vector2Int Position { get; protected set; }

        [SerializeField]
        public ENodeState state;

        public virtual bool IsObstacle
        {
            get => false;
        }

        /// <summary>
        /// ��㵽�õ�ľ��루·����ȷ����
        /// </summary>
        public float GCost;

        /// <summary>
        /// �õ㵽�յ�ľ��루�������ϰ���
        /// </summary>
        public float HCost;

        /// <summary>
        /// �����õ�ʱ����㵽�յ�ľ��루����õ㵽�յ����ϰ���
        /// </summary>
        public float FCost => process.HCostWeight * HCost + GCost;
        public float PrimitiveFCost => HCost + GCost;

        private AStarNode _Parent;
        public AStarNode Parent
        {
            get => _Parent;
            set
            {
                _Parent = value;
                GCost = value == null ? 0 : Parent.GCost + CalculateGCost(value);
            }
        }

        internal AStarNode(PathFindingProcess process, Vector2Int position)
        {
            this.process = process;
            Position = position;
            state = ENodeState.Blank;
        }

        public void UpdateHCost(AStarNode to)
        {
            HCost = CalculatePrimitiveCost(to);
        }

        public float CalculatePrimitiveCost(AStarNode other)
        {
            float distance = process.Settings.CalculateDistance(Position, other.Position);
            return distance;
        }
        protected virtual float CalculatePrimitiveCost(AStarNode other, float distance)
        {
            return distance;
        }
        public float CalculateGCost(AStarNode other)
        {
            return process.Settings.CalculateDistance(Position, other.Position);
        }
        protected virtual float CalculateGCost(AStarNode other, float distance)
        {
            return distance;
        }
        /// <summary>
        /// ����·��
        /// </summary>
        public void Recall(List<AStarNode> ret = null)
        {
            state = ENodeState.Route;
            Parent?.Recall(ret);
            ret?.Add(this);
        }

        public override string ToString()
        {
            return Position.ToString();
        }
    }

    public class Comparer_Cost : IComparer<AStarNode>
    {
        public int Compare(AStarNode x, AStarNode y)
        {
            return (int)Mathf.Sign(x.FCost - y.FCost);
        }
    }
}

