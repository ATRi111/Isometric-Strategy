using Services;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Move : Effect
{
    public Vector3Int from, to;
    public List<Vector3Int> route;

    public override bool Appliable => victim.GridPawn.CellPosition == from;
    public override bool Revokable => victim.GridPawn.CellPosition == to;

    public Effect_Move(PawnEntity victim, List<Vector3Int> route) 
        : base(victim)
    {
        this.route = route;
        from = route[0];
        to = route[^1];
    }

    public override void Apply()
    {
        base.Apply();
        MoveTo(to);
    }

    public override void Revoke()
    {
        base.Revoke();
        MoveTo(from);
    }

    private void MoveTo(Vector3Int destination)
    {
        AnimationManager manager = ServiceLocator.Get<AnimationManager>();
        if (manager.ImmediateMode)
        {
            victim.GridPawn.CellPosition = destination;
        }
        else
        {
            victim.MoveController.SetGridRoute(route);
        }
    }
}
