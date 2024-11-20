using Character;
using EditorExtend.GridEditor;

public class MovableGridObject : GridObject
{
    protected IsometricGridManager igm;
    private PawnEntity pawn;
    public AMover Mover { get; protected set; }
    public GridMoveController MoveController { get; protected set; }
    public override int ExtraSortingOrder => 5;

    public Property climbAbility;
    public Property dropAbility;
    public Property moveAbility;

    protected override void Awake()
    {
        base.Awake();
        igm = Manager as IsometricGridManager;
        pawn = GetComponentInParent<PawnEntity>();
        MoveController = GetComponentInChildren<GridMoveController>();
    }

    public void RefreshProperty()
    {
        climbAbility.Refresh();
        dropAbility.Refresh();
        moveAbility.Refresh();
        Mover = new AMover(this)
        {
            MoveAbility = () => moveAbility.IntValue
        };
    }

    public virtual bool StayCheck(ANode node)
    {
        GridObject obj = node.CurrentObject;
        if (obj == null)
            return false;
        
        if (obj is MovableGridObject)
            return false;
        return true;
    }

    public virtual bool MoveCheck(ANode from, ANode to)
    {
        GridObject obj = to.CurrentObject;
        if(obj == null)
            return false;

        if (obj is MovableGridObject other && !FactionCheck(other))
            return false;

        int toLayer = igm.AboveGroundLayer(to.Position);
        int fromLayer = igm.AboveGroundLayer(from.Position);
        if (!HeightCheck(fromLayer, toLayer))
            return false;
        return true;
    }

    public virtual bool FactionCheck(MovableGridObject other)
    {
        return pawn.faction == other.pawn.faction;
    }

    public virtual bool HeightCheck(int fromLayer, int toLayer)
    {
        return toLayer <= fromLayer + climbAbility.IntValue && toLayer >= fromLayer - dropAbility.IntValue;
    }
}
