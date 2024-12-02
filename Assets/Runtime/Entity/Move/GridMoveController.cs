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
    /// �����Ŷ��������ı�����λ��
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
        currentRoute[0] = igm.CellToWorld(route[0]);
        float length = 0;
        for (int i = 1; i < route.Count; i++)
        {
            currentRoute[i] = igm.CellToWorld(route[i]);
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
