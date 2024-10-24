using AStar;
using MyTool;
using Services;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private AIManager AI;
    private IsometricGridManager igm;
    public Pawn pawn;

    private void Awake()
    {
        AI = ServiceLocator.Get<AIManager>();
        igm = IsometricGridManager.FindInstance();
    }

    private void Start()
    {
        AI.PathFinding.AfterComplete += AfterComplete;
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            igm.MatchMaxLayer(world, out int layer);
            float z = igm.LayerToWorldZ(layer);
            Vector3Int cell = igm.WorldToCell(world.ResetZ(z));
            PathFindingProcess process = AI.PathFinding.FindRoute(pawn.Mover, (Vector2Int)pawn.CellPosition, (Vector2Int)cell);
            process.output.Log();
        }
        else if(Input.GetMouseButtonUp(1))
        {
            PathFindingProcess process = AI.PathFinding.FindAvailable(pawn.Mover, (Vector2Int)pawn.CellPosition);
            process.output.Log();
        }
    }

    private void AfterComplete(PathFindingProcess process)
    {
        List<Vector3Int> route = new()
        {
            pawn.CellPosition
        };
        for (int i = 0; i < process.output.Count; i++)
        {
            route.Add((process.output[i] as ANode).CellPositon);
        }
        pawn.MoveController.SetGridRoute(route);
    }
}
