using System;
using UnityEngine;

[Serializable]
public class TeleportEffect : Effect
{
    public Vector3Int from, to;

    public override bool Appliable => victim.GridObject.CellPosition == from;
    public override bool Revokable => victim.GridObject.CellPosition == to;

    public TeleportEffect(PawnEntity victim, Vector3Int from, Vector3Int to, int probability = MaxProbability)
        : base(victim, probability)
    {
        this.from = from;
        this.to = to;
    }

    public override AnimationProcess GenerateAnimation()
    {
        return null;
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
