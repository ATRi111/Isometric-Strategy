using AStar;
using Character;
using EditorExtend.GridEditor;
using Services;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

/// <summary>
/// 控制角色获取关于战场的信息
/// </summary>
public class PawnSensor : CharacterComponentBase
{
    private struct Pair
    {
        public Vector2Int from;
        public Vector2Int to;

        public override readonly bool Equals(object obj)
        {
            if(obj is Pair other)
            {
                return from == other.from && to == other.to;
            }
            return false;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(from, to);
        }
    }

    private AIManager AIManager;

    public PawnEntity Pawn => entity as PawnEntity;
    public readonly List<PawnEntity> allies = new();
    public readonly List<PawnEntity> enemies = new();
    private readonly Dictionary<Pair,float> distanceDict = new();

    public void Sense()
    {
        distanceDict.Clear();
        RecognizeEnemyAndAlly();
    }

    public void RecognizeEnemyAndAlly()
    {
        allies.Clear();
        enemies.Clear();
        foreach (PawnEntity pawn in Pawn.GameManager.pawns)
        {
            if (pawn == Pawn)
                continue;
            int flag = Pawn.FactionCheck(pawn);
            switch (flag)
            {
                case 1:
                    allies.Add(pawn);
                    break;
                case -1:
                    enemies.Add(pawn);
                    break;
                case 0:
                    break;
            }
        }
    }

    private void TryAdd(Pair pair,float distance)
    {
        if(distanceDict.ContainsKey(pair))
            distanceDict[pair] = Mathf.Min(distanceDict[pair], distance);
        else
            distanceDict.Add(pair, distance);
    }

    public int PredictDistanceBetween(Vector3Int from, Vector3Int to)
    {
        float HCost(Node node)
        {
            return IsometricGridUtility.ProjectManhattanDistance(node.Position, (Vector2Int)to);
        }
        float FCost(Node node)
        {
            return node.GCost + HCost(node);
        }

        Pair f2t = new()
        {
            from = (Vector2Int)from,
            to = (Vector2Int)to
        };
        if (!distanceDict.ContainsKey(f2t))
        {
            List<Node> available = new();
            List<Node> route = new();

            Ranging(from, available); 
            Node nearest = available[0];
            for (int i = 0; i < available.Count; i++)
            {
                Node node = available[i];
                Pair f2a = new()
                {
                    from = (Vector2Int)from,
                    to = node.Position
                };
                TryAdd(f2a, node.GCost);
                if (HCost(node) <= HCost(nearest) && FCost(node) < FCost(nearest))
                    nearest = node;
            }

            nearest.Recall(route);
            for (int i = 0; i < route.Count; i++)
            {
                Pair a2t = new()
                {
                    from = route[i].Position,
                    to = (Vector2Int)to
                };
                float distance = route[i].GCost - route[0].GCost + HCost(route[0]);
                TryAdd(a2t, distance);
            }
        }
        return Mathf.RoundToInt(distanceDict[f2t]);
    }

    /// <summary>
    /// 获取从from出发时，所有可达点
    /// </summary>
    public void FindAvailable(Vector3Int from, List<Vector3Int> ret)
    {
        Profiler.BeginSample("FindAvailable");
        ret.Clear();
        PathFindingProcess process = AIManager.PathFinding.FindAvailable(Pawn.MovableGridObject.Mover_Default, (Vector2Int)from);
        for (int i = 0; i < process.available.Count; i++)
        {
            ret.Add((process.available[i] as ANode).CellPosition);
        }
        Profiler.EndSample();
    }

    /// <summary>
    /// 计算from通往to的路径
    /// </summary>
    public void FindRoute(Vector3Int from, Vector3Int to, List<Vector3Int> ret)
    {
        Profiler.BeginSample("FindRoute");
        ret.Clear();
        ret.Add(Pawn.MovableGridObject.CellPosition);
        PathFindingProcess process = AIManager.PathFinding.FindRoute(Pawn.MovableGridObject.Mover_Default, (Vector2Int)from, (Vector2Int)to);
        for (int i = 0; i < process.output.Count; i++)
        {
            ret.Add((process.output[i] as ANode).CellPosition);
        }
        Profiler.EndSample();
    }

    /// <summary>
    /// 预测从from出发时，所有可达节点（忽略友方角色，考虑跳跃）
    /// </summary>
    public void Ranging(Vector3Int from, List<Node> ret)
    {
        Profiler.BeginSample("Ranging");
        ret.Clear();
        PathFindingProcess process = AIManager.PathFinding.Ranging(Pawn.MovableGridObject.Mover_Ranging, (Vector2Int)from);
        ret.AddRange(process.available);
        Profiler.EndSample();
    }

    protected override void Awake()
    {
        base.Awake();
        AIManager = ServiceLocator.Get<AIManager>();
    }
}
