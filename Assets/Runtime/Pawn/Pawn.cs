using EditorExtend.GridEditor;
using UnityEngine;

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

    public virtual bool StayCheck(ANode node)
    {
        GridObject obj = node.CurrentObject;
        if (obj == null)
            return false;
        
        if (obj is Pawn)
            return false;
        return true;
    }

    public virtual bool MoveCheck(ANode from, ANode to)
    {
        GridObject obj = to.CurrentObject;
        if(obj == null)
            return false;

        if (obj is Pawn other && !FactionCheck(other))
            return false;

        int toLayer = igm.AboveGroundLayer(to.Position);
        int fromLayer = igm.AboveGroundLayer(from.Position);
        if (!HeightCheck(fromLayer, toLayer))
            return false;
        return true;
    }

    public virtual bool FactionCheck(Pawn other)
    {
        return faction == other.faction;
    }

    public virtual bool HeightCheck(int fromLayer,int toLayer)
    {
        return toLayer <= fromLayer + climbAbility && toLayer >= fromLayer - dropAbility;
    }
}
