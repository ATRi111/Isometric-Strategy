using AStar;
using Character;
using EditorExtend.GridEditor;
using Services;
using Services.Event;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

/// <summary>
/// ���ƽ�ɫ��ȡ����ս������Ϣ
/// </summary>
public class PawnSensor : CharacterComponentBase
{
    private AIManager AIManager;

    public PawnEntity Pawn => entity as PawnEntity;

    private List<Vector2Int> adjacent = new();  //�����ĸ�

    public readonly List<PawnEntity> allies = new();
    public readonly List<PawnEntity> enemies = new();
    private readonly Dictionary<Vector2Int, List<Node>> nodeCache = new();

    public void Sense()
    {
        nodeCache.Clear();
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

    public int FCostOfNearest(Vector2Int from,Vector2Int to)
    {
        static float FCost(Node node, Vector2Int to)
        {
            return node.GCost + IsometricGridUtility.ProjectManhattanDistance(node.Position, to);
        }

        List<Node> nodes = nodeCache[from];
        Node nearest = nodes[0];
        float fCost = FCost(nearest, to);
        for (int i = 1; i < nodes.Count; i++)
        {
            if (nodes[i].GCost <= nearest.GCost)
            {
                nearest = nodes[i];
                fCost = Mathf.Min(fCost, FCost(nearest, to));
            }
        }
        return Mathf.RoundToInt(fCost);
    }

    public int PredictDistanceBetween(Vector2Int from, Vector2Int to)
    {
        if (!nodeCache.ContainsKey(from))
        {
            nodeCache.Add(from, new List<Node>());
            Ranging(from, nodeCache[from]);
        }
        return FCostOfNearest(from, to);
    }

    /// <summary>
    /// ��ȡ��from����ʱ�����пɴ��
    /// </summary>
    public void FindAvailable(Vector2Int from, List<Vector3Int> ret)
    {
        ret.Clear();
        PathFindingProcess process = AIManager.PathFinding.FindAvailable(Pawn.MovableGridObject.Mover_Default, from);
        for (int i = 0; i < process.available.Count; i++)
        {
            ret.Add((process.available[i] as ANode).CellPosition);
        }
    }

    /// <summary>
    /// ����fromͨ��to��·��
    /// </summary>
    public void FindRoute(Vector2Int from, Vector2Int to, List<Vector3Int> ret)
    {
        ret.Clear();
        ret.Add(Pawn.MovableGridObject.CellPosition);
        PathFindingProcess process = AIManager.PathFinding.FindRoute(Pawn.MovableGridObject.Mover_Default, from,to);
        for (int i = 0; i < process.output.Count; i++)
        {
            ret.Add((process.output[i] as ANode).CellPosition);
        }
    }

    /// <summary>
    /// Ԥ���from����ʱ�����пɴ�ڵ㣨�����ѷ���ɫ��������Ծ��
    /// </summary>
    public void Ranging(Vector2Int from, List<Node> ret)
    {
        Profiler.BeginSample("Ranging");
        ret.Clear();
        PathFindingProcess process = AIManager.PathFinding.Ranging(Pawn.MovableGridObject.Mover_Ranging, from);
        ret.AddRange(process.available);
        Profiler.EndSample();
    }

    private void BeforeDoAction(PawnEntity _)
    {
        Sense();
    }

    protected override void Awake()
    {
        base.Awake();
        AIManager = ServiceLocator.Get<AIManager>();
        adjacent = IsometricGridUtility.WithinProjectManhattanDistance(1);
    }

    private void OnEnable()
    {
        Pawn.EventSystem.AddListener<PawnEntity>(EEvent.BeforeDoAction, BeforeDoAction);    
    }

    private void OnDisable()
    {
        Pawn.EventSystem.RemoveListener<PawnEntity>(EEvent.BeforeDoAction, BeforeDoAction);
    }
}
