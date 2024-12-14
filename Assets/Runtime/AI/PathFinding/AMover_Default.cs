using AStar;
using EditorExtend.GridEditor;

public class AMover_Default : AMover
{
    public AMover_Default(MovableGridObject gridObject) 
        :base(gridObject)
    {

    }

    public override bool StayCheck(Node node)
    {
        if (!base.StayCheck(node))
            return false;

        GridObject obj = (node as ANode).CurrentObject;
        if (obj is MovableGridObject)
            return false;
        return true;
    }

    public override bool MoveCheck(Node from, Node to)
    {
        if (!base.MoveCheck(from, to))
            return false;

        GridObject obj = (to as ANode).CurrentObject;
        if (obj is MovableGridObject other && !gridObject.FactionCheck(other))
            return false;

        if (!gridObject.HeightCheck(from, to))
            return false;
        return true;
    }

    public override float CalculateCost(Node from, Node to, float primitiveCost)
    {
        float cost = primitiveCost * (to as ANode).difficulty;
        if (from is ANode aFrom && to is ANode aTo)
        {
            if (aFrom.AboveGroundLayer != aTo.AboveGroundLayer)
                cost += PathFindingUtility.Epsilon;     //����ѡ��ƽ��
        }
        return cost;
    }
}
