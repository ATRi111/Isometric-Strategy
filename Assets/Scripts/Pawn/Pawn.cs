using AStar;
using EditorExtend.GridEditor;

public class Pawn : GridObject
{
    private IsometricGridManager igm;
    public EFaction faction;
    public int climbAbility = 2;
    public int dropAbility = 3;

    protected virtual void Awake()
    {
        igm = Manager as IsometricGridManager;
    }

    public virtual bool StayCheck(AStarNode node)
    {
        GridObject obj = igm.GetObejectXY(node.Position);
        if (obj == null)
            return false;
        Pawn pawn = obj as Pawn;
        if (pawn != null && pawn.faction != faction)
            return false;
        return true;
    }

    public virtual bool MoveCheck(AStarNode from, AStarNode to)
    {
        int toLayer = igm.AboveGroundLayer(to.Position);
        int fromLayer = CellPosition.z;
        return toLayer <= fromLayer + climbAbility && toLayer >= fromLayer - dropAbility;
    }
}
