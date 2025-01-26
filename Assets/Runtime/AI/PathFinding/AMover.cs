using AStar;

public abstract class AMover : MoverBase
{
    protected MovableGridObject movable;
    public AMover(MovableGridObject movable) 
    {
        this.movable = movable;
    }

    public override bool StayCheck(Node node)
    {
        return base.StayCheck(node);
    }

    public override bool MoveCheck(Node from, Node to)
    {
        return base.StayCheck(to)
            && movable.HeightCheck(from, to);
    }

    public override float CalculateCost(Node from, Node to, float primitiveCost)
    {
        ANode aFrom = (ANode)from;
        ANode aTo = (ANode)to;
        float cost = primitiveCost * aTo.difficulty;
        if (aFrom.aboveGroundLayer != aTo.aboveGroundLayer)
            cost += PathFindingUtility.Epsilon;     //优先选择平地
        return cost;
    }
}
