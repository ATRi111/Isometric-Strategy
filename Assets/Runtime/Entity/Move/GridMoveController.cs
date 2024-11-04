using Character;
using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

public class GridMoveController : MoveController
{
    protected IsometricGridManager igm;
    [AutoComponent(EComponentPosition.SelfOrParent)]
    protected GridObject gridObject;
    /// <summary>
    /// 仅播放动画，不改变网格位置
    /// </summary>
    public bool animationOnly = true;

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
