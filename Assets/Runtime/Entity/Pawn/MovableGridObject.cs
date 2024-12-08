using AStar;
using Character;
using EditorExtend.GridEditor;

public class MovableGridObject : GridObject
{
    public IsometricGridManager Igm { get; protected set; }
    public PawnEntity Pawn { get; protected set; }
    public AMover Mover_Default { get; protected set; }
    public AMover Mover_IgnorePawn { get; protected set; }
    public GridMoveController MoveController { get; protected set; }
    public override int ExtraSortingOrder => 5;

    public CharacterProperty climbAbility;
    public CharacterProperty dropAbility;
    public CharacterProperty moveAbility;

    protected override void Awake()
    {
        base.Awake();
        Igm = Manager as IsometricGridManager;
        Pawn = GetComponentInParent<PawnEntity>();
        MoveController = GetComponentInChildren<GridMoveController>();
        Mover_Default = new AMover_Default(this)
        {
            MoveAbility = () => moveAbility.IntValue
        };
        Mover_IgnorePawn = new AMover_IgnorePawn(this)
        {
            MoveAbility = () => 5 * moveAbility.IntValue 
        };
    }

    public void RefreshProperty()
    {
        climbAbility.Refresh();
        dropAbility.Refresh();
        moveAbility.Refresh();
    }

    public virtual bool FactionCheck(MovableGridObject other)
    {
        return Pawn.faction == other.Pawn.faction;
    }

    public virtual bool HeightCheck(AStarNode from, AStarNode to)
    {
        int toLayer = Igm.AboveGroundLayer(to.Position);
        int fromLayer = Igm.AboveGroundLayer(from.Position);
        return HeightCheck(fromLayer, toLayer);
    }

    public virtual bool HeightCheck(int fromLayer, int toLayer)
    {
        return toLayer <= fromLayer + climbAbility.IntValue && toLayer >= fromLayer - dropAbility.IntValue;
    }
}
