using AStar;

/// <summary>
/// 用于测距，移动及停留时忽略Pawn，跳跃时忽略友方Pawn
/// </summary>
public class AMover_Ranging : AMover
{
    public AMover_Ranging(MovableGridObject gridObject) 
        : base(gridObject)
    {
    }

    public override bool MoveCheck(Node from, Node to)
    {
        if ((from.Position - to.Position).sqrMagnitude == 1)
            return base.MoveCheck(from, to);

        return gridObject.JumpCheck(from.Position, to.Position, MovableGridObject.ObjectCheck_IgnoreAlly);
    }

    public override bool MoveAbilityCheck(Node node)
    {
        return true;    //测距时无需考虑移动力限制
    }
}
