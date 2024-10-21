using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public class AStarNode
    {
        protected internal readonly PathFindingProcess process;

        public Vector2Int Position { get; protected set; }

        [SerializeField]
        public ENodeState state;

        public virtual bool IsObstacle
        {
            get => false;
        }

        /// <summary>
        /// 起点到该点的距离（路径已确定）
        /// </summary>
        public float GCost;

        /// <summary>
        /// 该点到终点的距离（假设无障碍）
        /// </summary>
        public float HCost;

        public float WeightedFCost => process.HCostWeight * HCost + GCost;
        public float FCost => HCost + GCost;

        private AStarNode parent;
        public AStarNode Parent
        {
            get => parent;
            set
            {
                parent = value;
                GCost = value == null ? 0 : Parent.GCost + value.CostTo(this);
            }
        }

        internal AStarNode(PathFindingProcess process, Vector2Int position)
        {
            this.process = process;
            Position = position;
            state = ENodeState.Blank;
        }


        public float CostTo(AStarNode to)
        {
            float primitiveCost = PrimitiveCostTo(to);
            return process.mover.CalculateCost(this, to, primitiveCost);
        }
        protected internal virtual float PrimitiveCostTo(AStarNode to)
        {
            float distance = process.Settings.CalculateDistance(Position, to.Position);
            return distance;
        }

        /// <summary>
        /// 回溯路径
        /// </summary>
        public void Recall(List<AStarNode> ret = null)
        {
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
            return (int)Mathf.Sign(x.WeightedFCost - y.WeightedFCost);
        }
    }
}

