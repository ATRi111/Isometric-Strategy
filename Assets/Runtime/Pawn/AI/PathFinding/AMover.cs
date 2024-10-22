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
        return base.StayCheck(node) && pawn.StayCheck(node);
    }

    public override bool MoveCheck(AStarNode from, AStarNode to)
    {
        return base.MoveCheck(from, to) && pawn.MoveCheck(from, to);
    }

    public override float CalculateCost(AStarNode from, AStarNode to, float primitiveCost)
    {
        return primitiveCost * (to as ANode).difficulty;
    }
}
