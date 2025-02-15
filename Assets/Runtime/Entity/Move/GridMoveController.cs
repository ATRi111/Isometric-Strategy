using System.Collections.Generic;
using UnityEngine;

public class GridMoveController : MoveController
{
    protected IsometricGridManager Igm => IsometricGridManager.Instance;

    public float MockTime_CellPosition(List<Vector3> route, float speed)
    {
        List<Vector3> worldRoute = new();
        worldRoute.AddRange(route);
        for (int i = 0; i < worldRoute.Count; i++)
        {
            worldRoute[i] = Igm.CellToWorld(worldRoute[i]);
        }
        return CalculateLength(worldRoute) / speed;
    }

    public void SetRoute_CellPosition(List<Vector3> route, float speed)
    {
        if(route.Count < 2)
            return;

        currentRoute = new Vector3[route.Count];
        currentRoute[0] = Igm.CellToWorld(route[0]);
        float length = 0;
        for (int i = 1; i < route.Count; i++)
        {
            currentRoute[i] = Igm.CellToWorld(route[i]);
            length += (currentRoute[i] - currentRoute[i - 1]).magnitude;
        }
        ufm.Initialize(currentRoute, length, length / speed);
    }
}
