using AStar;
using MyTimer;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathFindingManager : MonoBehaviour
{
    private IsometricGridManager Igm => IsometricGridManager.Instance;

    [SerializeField]
    private PathFindingProcess findRoute;
    [SerializeField]
    private PathFindingProcess findAvailable;
    [SerializeField]
    private PathFindingProcess ranging;

    public PathFindingProcess FindRoute(AMover mover, Vector2Int from, Vector2Int to)
    {
        int maxDepth = Igm.MaxLayerDict.Count;
        findRoute.settings.maxDepth = maxDepth;
        findRoute.settings.capacity = maxDepth;
        findRoute.mountPoint = Igm.transform;
        findRoute.mover = mover;
        findRoute.Start(from, to);
#if UNITY_EDITOR
        if (debug)
            Play(findRoute);
        else
#endif
            findRoute.Complete();
        return findRoute;
    }

    public PathFindingProcess FindAvailable(AMover mover, Vector2Int from)
    {
        float moveability = mover.GetMoveAbility();
        int maxDepth = Mathf.CeilToInt(moveability * moveability * 2 + moveability * 2 + 2);
        findAvailable.settings.maxDepth = maxDepth;
        findAvailable.settings.capacity = maxDepth;
        findAvailable.mountPoint = Igm.transform;
        findAvailable.mover = mover;
        Vector2Int to = from + Mathf.CeilToInt(moveability + 1) * Vector2Int.one;
        findAvailable.Start(from, to);
#if UNITY_EDITOR
        if (debug)
            Play(findAvailable);
        else
#endif
            findAvailable.Complete();
        return findAvailable;
    }

    public PathFindingProcess Ranging(AMover mover, Vector2Int from)
    {
        float moveability = mover.GetMoveAbility();
        int maxDepth = Mathf.CeilToInt(moveability * moveability * 2 + moveability * 2 + 2);
        ranging.settings.maxDepth = maxDepth;
        ranging.settings.capacity = maxDepth;
        ranging.mountPoint = Igm.transform;
        ranging.mover = mover;
        Vector2Int to = from + Mathf.CeilToInt(moveability + 1) * Vector2Int.one;
        ranging.Start(from, to);
#if UNITY_EDITOR
        if (debug)
            Play(ranging);
        else
#endif
            ranging.Complete();
        return ranging;
    }

    public static void GetAdjoinNodes(PathFindingProcess process, Node node, List<Node> ret)
    {
        PathFindingUtility.GetAdjoinNodes_Four(process, node, ret);
    }

    public Node GenerateNode(PathFindingProcess process, Vector2Int position)
    {
        return new ANode(process, position, Igm, 1f);   //TODO:À§ÄÑµØÐÎ
    }


#if UNITY_EDITOR
    public UnityAction<PathFindingProcess> AfterStep;
    public UnityAction<PathFindingProcess> AfterComplete;

    public bool debug;
    public float stepTime;

    private Metronome metronome;
    private PathFindingProcess current;

    private void Play(PathFindingProcess process)
    {
        if (metronome != null)
            metronome.Paused = true;
        metronome = new Metronome();
        metronome.AfterComplete += Step;
        metronome.Initialize(stepTime);
        current = process;
        AfterStep?.Invoke(process);
    }

    private void Step(float _)
    {
        if (!current.NextStep())
        {
            metronome.Paused = true;
            AfterComplete?.Invoke(current);
            current = null;
        }
        AfterStep?.Invoke(current);
    }
#endif
}
