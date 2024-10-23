using AStar;
using EditorExtend.GridEditor;

public class Pawn : GridObject
{
    protected IsometricGridManager igm;
    public AMover Mover { get; protected set; }

    public EFaction faction;
    public int climbAbility = 2;
    public int dropAbility = 3;
    public float moveAbility = 5;

    protected virtual void Awake()
    {
        igm = Manager as IsometricGridManager;
        Mover = new AMover(this, moveAbility);
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
        int fromLayer = igm.AboveGroundLayer(from.Position);
        return toLayer <= fromLayer + climbAbility && toLayer >= fromLayer - dropAbility;
    }
}
