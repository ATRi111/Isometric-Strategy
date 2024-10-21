using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AStar
{
    /// <summary>
    /// 一次寻路过程
    /// </summary>
    [Serializable]
    public class PathFindingProcess
    {
        [SerializeField]
        private PathFindingSettings settings;
        public PathFindingSettings Settings => settings;
        public MonoBehaviour mono;

        private List<AStarNode> output;

        #region 基础方法

        /// <summary>
        /// 获取地图上某个位置的节点，并自动确定其节点类型
        /// </summary>
        internal AStarNode GetNode(Vector2Int pos)
        {
            if (discoveredNodes.ContainsKey(pos))
                return discoveredNodes[pos];
            AStarNode node = settings.GenerateNode(this, pos);
            discoveredNodes.Add(pos, node);
            countOfQuery++;
            return node;
        }

        /// <summary>
        /// 获取与一个节点相邻且可通行且不为Close的节点
        /// </summary>
        internal void GetAdjoinPassableNodes(AStarNode from)
        {
            adjoins_original.Clear();
            adjoins_handled.Clear();
            settings.GetAdjoinNodes.Invoke(this, from, adjoins_original);
            foreach (AStarNode to in adjoins_original)
            {
                if (to.state != ENodeState.Close && settings.mover.MoveCheck(this, from, to))
                    adjoins_handled.Add(to);
            }
        }

        public AStarNode[] GetAllNodes()
        {
            return discoveredNodes.Values.ToArray();
        }
        #endregion

        #region 状态量
        [SerializeField]
        private bool isRunning;
        /// <summary>
        /// 是否正在进行寻路
        /// </summary>
        public bool IsRunning => isRunning;

        /// <summary>
        /// 起点
        /// </summary>
        public AStarNode From { get; private set; }
        /// <summary>
        /// 终点
        /// </summary>
        public AStarNode To { get; private set; }
        /// <summary>
        /// 所有已发现节点
        /// </summary>
        internal readonly Dictionary<Vector2, AStarNode> discoveredNodes = new Dictionary<Vector2, AStarNode>();

        private readonly List<AStarNode> adjoins_original = new List<AStarNode>();
        private readonly List<AStarNode> adjoins_handled = new List<AStarNode>();
        /// <summary>
        /// 待访问节点表
        /// </summary>
        internal Heap<AStarNode> open;

        /// <summary>
        /// 当前访问的点
        /// </summary>
        [SerializeField]
        internal AStarNode currentNode;
        public AStarNode CurrentNode => currentNode;

        internal AStarNode nearest;
        /// <summary>
        /// 当前已访问的离终点最近的点
        /// </summary>
        public AStarNode Nearest => nearest;
        [SerializeField]
        internal int countOfTestedNode;
        /// <summary>
        /// 测试过的节点数
        /// </summary>
        public int CountOfTestedNode => countOfTestedNode;

        [SerializeField]
        internal int countOfQuery;
        /// <summary>
        /// 查询节点次数
        /// </summary>
        public int CountOfQuery => countOfQuery;

        [SerializeField]
        internal float currentWeight;
        /// <summary>
        /// 寻路的当前一步中,HCost的权重
        /// </summary>
        public float CurrentWeight => currentWeight;

        #endregion

        #region 运行过程
        /// <summary>
        /// 开始寻路
        /// </summary>
        /// <param name="fromPos">起点</param>
        /// <param name="toPos">终点</param>
        /// <param name="ret">接收结果</param>
        public void Start(Vector2Int fromPos, Vector2Int toPos, List<AStarNode> ret = null)
        {
            if (fromPos == toPos)
            {
                Debug.LogWarning("起点与终点相同");
                return;
            }

            isRunning = true;
            countOfQuery = 0;
            countOfTestedNode = 0;
            currentWeight = 1f;

            discoveredNodes.Clear();
            open = new Heap<AStarNode>(settings.capacity, new Comparer_Cost());
            output = ret;

            To = GetNode(toPos);
            To.state = ENodeState.Route;

            From = GetNode(fromPos);
            From.state = ENodeState.Route;
            From.Parent = null;
            From.UpdateHCost(To);

            open.Push(From);
            nearest = From;
        }
        /// <summary>
        /// 立刻完成寻路
        /// </summary>
        public void Compelete()
        {
            for (; ; )
            {
                if (!NextStep())
                    return;
            }
        }
        /// <summary>
        /// 进行一步寻路
        /// </summary>
        public bool NextStep()
        {
            if (!CheckNextStep())
            {
                if (isRunning)
                    Stop();
                return false;
            }

            currentNode = open.Pop();
            currentNode.state = ENodeState.Close;
            GetAdjoinPassableNodes(currentNode);

            foreach (AStarNode node in adjoins_handled)
            {
                switch (node.state)
                {
                    case ENodeState.Blank:
                        node.UpdateHCost(To);
                        node.Parent = currentNode;
                        node.state = ENodeState.Open;
                        open.Push(node);
                        break;
                    case ENodeState.Route:
                        node.Parent = currentNode;
                        nearest = node;
                        Stop();
                        return false;
                    case ENodeState.Open:
                        if (node.GCost > currentNode.GCost + currentNode.CalculateGCost(node))
                            node.Parent = currentNode;
                        break;
                }
                if (node.HCost < nearest.HCost)
                    nearest = node;
                countOfTestedNode++;
            }
            return true;
        }
        /// <summary>
        /// 停止寻路并返回结果
        /// </summary>
        public void Stop()
        {
            isRunning = false;
            nearest.Recall(output);
        }

        private bool CheckNextStep()
        {
            if (!isRunning)
            {
                Debug.LogWarning("寻路未开始");
                return false;
            }
            if (countOfTestedNode > settings.maxDepth)
            {
                Debug.LogWarning("超出步数限制");
                return false;
            }
            if (open.IsEmpty)
            {
                Debug.LogWarning("找不到路径");
                return false;
            }
            return true;
        }
        #endregion

        public PathFindingProcess(PathFindingSettings settings)
        {
            this.settings = settings;
        }
    }
}