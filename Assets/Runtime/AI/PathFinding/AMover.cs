using AStar;
using EditorExtend.GridEditor;

public abstract class AMover : MoverBase
{
    protected MovableGridObject gridObject;
    public AMover(MovableGridObject gridObject) 
    {
        this.gridObject = gridObject;
    }

    /// <summary>
    /// 判断最终能否停留在某个节点
    /// </summary>
    public override bool StayCheck(Node node)
    {
        if (node.IsObstacle)
            return false;
        GridObject obj = (node as ANode).CurrentObject;
        if (obj == null)
            return false;

        return true;
    }

    /// <summary>
    /// 判断能否从某节点移动到另一节点
    /// </summary>
    public override bool MoveCheck(Node from, Node to)
    {
        if (to.IsObstacle)
            return false;
        GridObject obj = (to as ANode).CurrentObject;
        if (obj == null)
            return false;
        if (!gridObject.HeightCheck(from, to))
            return false;

        return true;
    }

    /// <summary>
    /// 在原始距离的基础上，计算两点间距离
    /// </summary>
    public override float CalculateCost(Node from, Node to, float primitiveCost)
    {
        float cost = primitiveCost * (to as ANode).difficulty;
        if (from is ANode aFrom && to is ANode aTo)
        {
            if (aFrom.AboveGroundLayer != aTo.AboveGroundLayer)
                cost += PathFindingUtility.Epsilon;     //优先选择平地
        }
        return cost;
    }
}
