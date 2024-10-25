using Character;
using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

public class GridMoveController : MoveController
{
    protected IsometricGridManager igm;
    [AutoComponent(EComponentPosition.SelfOrParent)]
    protected GridObject gridObject;

    protected override void Awake()
    {
        base.Awake();
        igm = IsometricGridManager.FindInstance();
    }

    public void SetGridRoute(List<Vector3Int> route)
    {
        if(route.Count < 2)
            return;

        currentRoute = new Vector3[route.Count];
        for (int i = 0; i < route.Count; i++)
        {
            currentRoute[i] = igm.CellToWorld(route[i]);
        }
        currentIndex = 1;
        MoveTo(currentRoute[currentIndex]);
    }

    protected override void AfterComplete(Vector3 v)
    {
        base.AfterComplete(v);
        gridObject.AlignXY();
    }

    public override void ForceComplete()
    {
        base.ForceComplete();
        gridObject.AlignXY();
    }
}
