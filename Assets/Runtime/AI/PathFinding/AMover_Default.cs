using AStar;

public class AMover_Default : AMover
{
    public AMover_Default(MovableGridObject movable) 
        :base(movable)
    {

    }

    public override bool StayCheck(Node node)
    {
        return base.StayCheck(node)
            && !((ANode)node).isPawn;
    }

    public override bool MoveCheck(Node from, Node to)
    {
        ANode aTo = (ANode)to;
        return base.MoveCheck(from, to)
            && (!aTo.isPawn || movable.FactionCheck(aTo.movableGridObject));    //移动时可穿越友方
    }
}
