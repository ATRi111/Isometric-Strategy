using EditorExtend.GridEditor;
using MyTool;
using UnityEngine;

public class GridObjectMoveController : GridMoveController
{
    protected PawnEntity pawnEntity;
    protected GridObject gridObject;
    /// <summary>
    /// �����Ŷ��������ı�����λ��
    /// </summary>
    public bool animationOnly = true;

    protected override void OnTick(Vector3 v)
    {
        if(pawnEntity != null)
        {
            Vector3 delta = v - Position;
            Vector2 cell = IsometricGridUtility.WorldToCell(delta, Igm.Grid.cellSize);
            if (cell != Vector2.zero)
                pawnEntity.faceDirection = EDirectionTool.NearestDirection4(cell);
        }
        base.OnTick(v);
    }

    protected override void AfterComplete(Vector3 v)
    {
        base.AfterComplete(v);
        //�ƶ�����ʱǿ�Ƹ�ԭΪIdle�������ƶ�ʱ�Ķ���ȡ���ڼ��ܣ�������һ��⵽�ƶ����Զ��л������߶�����
        pawnEntity.PawnAnimator.Play(EPawnAnimationState.Idle.ToString());
        if (!animationOnly)
            gridObject.AlignXY();
    }

    public override void ForceComplete()
    {
        base.ForceComplete();
        if(!animationOnly)
            gridObject.AlignXY(); 
    }

    protected override void Awake()
    {
        base.Awake();
        gridObject = GetComponentInParent<GridObject>();
        pawnEntity = GetComponentInParent<PawnEntity>();
        target = gridObject.transform;
    }
}
