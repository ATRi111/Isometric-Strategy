using AStar;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PathFindingManager
{
    private IsometricGridManager igm;
    private IsometricGridManager Igm
    {
        get
        {
            if (igm == null)
                igm = IsometricGridManager.FindInstance();
            return igm;
        }
    }

    [SerializeField]
    private PathFindingProcess findRoute;
    [SerializeField]
    private PathFindingProcess findAvailable;

    public PathFindingProcess FindRoute(AMover mover, Vector2Int from, Vector2Int to)
    {
        PathFindingSettings settings = new(GetAdjoinNodes, PathFindingUtility.ManhattanDistance, GenerateNode, 1f, 100, 100);
        findRoute = new PathFindingProcess(settings, mover);
        findRoute.Start(from, to);
        findRoute.Compelete();
        return findRoute;
    }

    public PathFindingProcess FindAvailable(AMover mover,Vector2Int from)
    {
        int maxDepth = Mathf.CeilToInt(mover.moveAbility * mover.moveAbility * 2 + mover.moveAbility * 4 + 8);
        PathFindingSettings settings = new(GetAdjoinNodes, PathFindingUtility.ManhattanDistance, GenerateNode, 0f, maxDepth, maxDepth);
        findAvailable = new PathFindingProcess(settings, mover);
        Vector2Int to = from + Mathf.CeilToInt(mover.moveAbility + 1) * Vector2Int.one;
        findAvailable.Start(from, to);
        findAvailable.Compelete();
        return findAvailable;
    }

    public static void GetAdjoinNodes(PathFindingProcess process, AStarNode node, List<AStarNode> ret)
    {
        PathFindingUtility.GetAdjoinNodes_Four(process, node, ret);
    }

    public AStarNode GenerateNode(PathFindingProcess process, Vector2Int position)
    {
        return new ANode(process, position, Igm, 1f);   //困难地形待完成
    }
}
