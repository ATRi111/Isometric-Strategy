using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MoveEffect : Effect
{
    public Vector3Int from, to;
    public readonly List<Vector3Int> route;

    public override bool Appliable => victim.GridObject.CellPosition == from;
    public override bool Revokable => victim.GridObject.CellPosition == to;

    public MoveEffect(PawnEntity victim, List<Vector3Int> route, int probability = MaxProbability)
        : base(victim, probability)
    {
        this.route = new();
        this.route.AddRange(route);
        from = route[0];
        to = route[^1];
    }

    public override AnimationProcess GenerateAnimation()
    {
        return new AnimationProcess_Move(this);
    }

    public override void Apply()
    {
        base.Apply();
        victim.GridObject.CellPosition = to;
    }

    public override void Revoke()
    {
        base.Revoke();
        victim.GridObject.CellPosition = from;
    }

    public override float PrimitiveValueFor(PawnEntity pawn)
    {
        return pawn.Brain.EvaluatePosition(to) - pawn.Brain.EvaluatePosition(from);
    }
}