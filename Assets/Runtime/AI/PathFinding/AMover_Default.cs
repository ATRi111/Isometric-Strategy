using AStar;

public class AMover_Default : AMover
{
    public AMover_Default(MovableGridObject movable)
        : base(movable)
    {

    }

    public override bool StayCheck(Node node)
    {
        return base.StayCheck(node)
            && !((ANode)node).isEntity;
    }

    public override bool MoveCheck(Node from, Node to)
    {
        ANode aTo = (ANode)to;
        return base.MoveCheck(from, to)
            && (!aTo.isEntity || movable.FactionCheck(aTo.entity));    //移动时可穿越友方
    }
}
