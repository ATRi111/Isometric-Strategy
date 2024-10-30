using EditorExtend.GridEditor;

public class GridPawn : GridObject
{
    protected IsometricGridManager igm;
    private PawnEntity pawn;
    public AMover Mover { get; protected set; }
    public GridMoveController MoveController { get; protected set; }
    public override int ExtraSortingOrder => 5;

    protected virtual void Awake()
    {
        igm = Manager as IsometricGridManager;
        pawn = GetComponentInParent<PawnEntity>();
        MoveController = GetComponentInChildren<GridMoveController>();
        Mover = new AMover(this, pawn.Property.moveAbility);
    }

    public virtual bool StayCheck(ANode node)
    {
        GridObject obj = node.CurrentObject;
        if (obj == null)
            return false;
        
        if (obj is GridPawn)
            return false;
        return true;
    }

    public virtual bool MoveCheck(ANode from, ANode to)
    {
        GridObject obj = to.CurrentObject;
        if(obj == null)
            return false;

        if (obj is GridPawn other && !FactionCheck(other))
            return false;

        int toLayer = igm.AboveGroundLayer(to.Position);
        int fromLayer = igm.AboveGroundLayer(from.Position);
        if (!HeightCheck(fromLayer, toLayer))
            return false;
        return true;
    }

    public virtual bool FactionCheck(GridPawn other)
    {
        return pawn.Setting.faction == other.pawn.Setting.faction;
    }

    public virtual bool HeightCheck(int fromLayer,int toLayer)
    {
        return toLayer <= fromLayer + pawn.Property.climbAbility && toLayer >= fromLayer - pawn.Property.dropAbility;
    }
}
