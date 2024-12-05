using AStar;
using MyTimer;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathFindingManager : MonoBehaviour
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
        int maxDepth = Igm.MaxLayerDict.Count;
        PathFindingSettings settings = new(GetAdjoinNodes, PathFindingUtility.ManhattanDistance, GenerateNode, 1f, maxDepth, maxDepth);
        findRoute = new PathFindingProcess(settings, mover)
        {
            mono = Igm
        };
        findRoute.Start(from, to);
#if UNITY_EDITOR
        if(debug)
            PlayFindRoute();
        else
#endif
            findRoute.Compelete();
        return findRoute;
    }

    public PathFindingProcess FindAvailable(AMover mover,Vector2Int from)
    {
        float moveability = mover.MoveAbility();
        int maxDepth = Mathf.CeilToInt(moveability * moveability * 2 + moveability * 2 + 2);
        PathFindingSettings settings = new(GetAdjoinNodes, PathFindingUtility.ManhattanDistance, GenerateNode, 0f, maxDepth, maxDepth);
        findAvailable = new PathFindingProcess(settings, mover)
        {
            mono = Igm
        };
        Vector2Int to = from + Mathf.CeilToInt(moveability + 1) * Vector2Int.one;
        findAvailable.Start(from, to);
#if UNITY_EDITOR
        if (debug)
            PlayFindAvailable();
        else
#endif
            findAvailable.Compelete();
        return findAvailable;
    }

    public static void GetAdjoinNodes(PathFindingProcess process, AStarNode node, List<AStarNode> ret)
    {
        PathFindingUtility.GetAdjoinNodes_Four(process, node, ret);
    }

    public AStarNode GenerateNode(PathFindingProcess process, Vector2Int position)
    {
        return new ANode(process, position, Igm, 1f);   //TODO:À§ÄÑµØÐÎ
    }


#if UNITY_EDITOR
    public UnityAction<PathFindingProcess> AfterStep;
    public UnityAction<PathFindingProcess> AfterComplete;

    public bool debug;
    public float stepTime;

    private Metronome metronome;

    private void PlayFindRoute()
    {
        if (metronome != null)
            metronome.Paused = true;
        metronome = new Metronome();
        metronome.AfterComplete += FindRouteStep;
        metronome.Initialize(stepTime);
        AfterStep?.Invoke(findRoute);
    }

    private void FindRouteStep(float _)
    {
        if (!findRoute.NextStep())
        {
            metronome.Paused = true;
            AfterComplete?.Invoke(findRoute);
        }
        AfterStep?.Invoke(findRoute);
    }

    private void PlayFindAvailable()
    {
        if (metronome != null)
            metronome.Paused = true;
        metronome = new Metronome();
        metronome.AfterComplete += FindAvailableNextStep;
        metronome.Initialize(stepTime);
        AfterStep?.Invoke(findAvailable);
    }

    private void FindAvailableNextStep(float _)
    {
        if (!findAvailable.NextStep())
        {
            metronome.Paused = true;
            AfterComplete?.Invoke(findAvailable);
        }
        AfterStep?.Invoke(findAvailable);
    }
#endif
}
