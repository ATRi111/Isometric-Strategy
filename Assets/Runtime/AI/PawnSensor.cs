using AStar;
using Character;
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

    public void Sense()
    {
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

    public void Ranging(Vector3Int from, Dictionary<Vector2Int, ANode> ret)
    {
        Profiler.BeginSample("Ranging");
        ret.Clear();
        PathFindingProcess process = AIManager.PathFinding.Ranging(Pawn.MovableGridObject.Mover_Ranging, (Vector2Int)from);
        for (int i = 0; i < process.available.Count; i++)
        {
            ANode node = process.available[i] as ANode;
            ret.Add(node.Position, node);
        }
        Profiler.EndSample();
    }

    protected override void Awake()
    {
        base.Awake();
        AIManager = ServiceLocator.Get<AIManager>();
    }
}
