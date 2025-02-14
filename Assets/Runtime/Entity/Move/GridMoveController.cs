using Character;
using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

public class GridMoveController : MoveController
{
    private IsometricGridManager Igm => IsometricGridManager.Instance;

    [AutoComponent(EComponentPosition.SelfOrParent)]
    protected GridObject gridObject;
    /// <summary>
    /// 仅播放动画，不改变网格位置
    /// </summary>
    public bool animationOnly = true;

    public float MockTime(List<Vector3> route, float speed)
    {
        if (route.Count < 2)
            return 0f;
        Vector3 prev = route[0];
        Vector3 current;
        float length = 0;
        for (int i = 1; i < route.Count; i++)
        {
            current = Igm.CellToWorld(route[i]);
            length += (current - prev).magnitude;
            prev = current;
        }
        return length / speed;
    }

    public void SetGridRoute(List<Vector3> route, float speed)
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

    protected override void AfterComplete(Vector3 v)
    {
        base.AfterComplete(v);
        if (!animationOnly)
            gridObject.AlignXY();
    }

    public override void ForceComplete()
    {
        base.ForceComplete();
        if(!animationOnly)
            gridObject.AlignXY(); 
    }
}
