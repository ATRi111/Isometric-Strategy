using EditorExtend.GridEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MoveEffect : Effect
{
    public Vector3Int from, to;
    public readonly List<Vector3> route;

    public override bool Appliable => victim.GridObject.CellPosition == from;
    public override bool Revokable => victim.GridObject.CellPosition == to;

    public MoveEffect(PawnEntity victim, Vector3Int from, Vector3Int to, List<Vector3> route, int probability = MaxProbability)
        : base(victim, probability)
    {
        this.route = new();
        this.route.AddRange(route);
        this.from = from;
        this.to = to;
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
