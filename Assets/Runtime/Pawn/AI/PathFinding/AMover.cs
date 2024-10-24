using AStar;

public class AMover : AStarMover
{
    protected Pawn pawn;
    public AMover(Pawn pawn, float moveAbility = float.PositiveInfinity) 
        : base(moveAbility)
    {
        this.pawn = pawn;
    }

    public override bool StayCheck(AStarNode node)
    {
        return base.StayCheck(node) && pawn.StayCheck(node as ANode);
    }

    public override bool MoveCheck(AStarNode from, AStarNode to)
    {
        return base.MoveCheck(from, to) && pawn.MoveCheck(from as ANode, to as ANode);
    }

    public override float CalculateCost(AStarNode from, AStarNode to, float primitiveCost)
    {
        float cost = primitiveCost * (to as ANode).difficulty;
        if (from is ANode aFrom && to is ANode aTo)
        {
            if (aFrom.AboveGroundLayer != aTo.AboveGroundLayer)
                cost += PathFindingUtility.Epsilon;
        }
        return cost;
    }
}
