using Character;
using EditorExtend.GridEditor;
using UnityEngine;

public class GridObjectMoveController : GridMoveController
{
    [AutoComponent(EComponentPosition.SelfOrParent)]
    protected GridObject gridObject;
    /// <summary>
    /// 仅播放动画，不改变网格位置
    /// </summary>
    public bool animationOnly = true;

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
