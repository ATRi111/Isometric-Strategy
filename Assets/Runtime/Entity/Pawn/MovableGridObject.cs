using AStar;
using Character;
using EditorExtend.GridEditor;
using System;
using UnityEngine;

public class MovableGridObject : GridObject
{
    public static bool ObjectCheck_AllObject(PawnEntity pawn,GridObject gridObject)
    {
        return true;
    }
    public static bool ObjectCheck_IgnoreAlly(PawnEntity pawn, GridObject gridObject)
    {
        Entity entity = gridObject.GetComponentInParent<Entity>();
        if(entity != null && pawn.FactionCheck(entity) > 0)
            return false;
        return true;
    }

    public IsometricGridManager Igm { get; protected set; }
    public PawnEntity Pawn { get; protected set; }
    public AMover Mover_Default { get; protected set; }
    public AMover Mover_Ranging { get; protected set; }
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
            GetMoveAbility = () => moveAbility.IntValue
        };
        Mover_Ranging = new AMover_Ranging(this)
        {
            GetMoveAbility = () => 5 * moveAbility.IntValue 
        };
    }

    public void RefreshProperty()
    {
        climbAbility.Refresh();
        dropAbility.Refresh();
        moveAbility.Refresh();
    }

    public virtual bool JumpCheck(Vector2Int fromXY, Vector2Int toXY, Func<PawnEntity,GridObject, bool> ObjectCheck = null)
    {
        JumpSkill jumpSkill = Pawn.Brain.FindSkill<JumpSkill>();
        if (jumpSkill == null)
            return false;
        return jumpSkill.JumpCheck(Pawn, Pawn.Igm, fromXY, toXY, ObjectCheck);
    }

    public virtual bool FactionCheck(MovableGridObject other)
    {
        return Pawn.faction == other.Pawn.faction;
    }

    public virtual bool HeightCheck(Node from, Node to)
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
