using EditorExtend.GridEditor;
using MyTool;
using UnityEngine;

public class GridObjectMoveController : GridMoveController
{
    protected PawnEntity pawnEntity;
    protected GridObject gridObject;
    /// <summary>
    /// 仅播放动画，不改变网格位置
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
        //移动结束时强制复原为Idle动作（移动时的动作取决于技能，并不会一检测到移动就自动切换到行走动作）
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
