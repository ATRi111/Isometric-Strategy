using System;
using System.Text;
using UnityEngine;

[Serializable]
public class TeleportEffect : Effect
{
    public Vector3Int from, to;

    public override bool Appliable => victim.GridObject.CellPosition == from;

    public TeleportEffect(PawnEntity victim, Vector3Int from, Vector3Int to, int probability = MaxProbability)
        : base(victim, probability)
    {
        this.from = from;
        this.to = to;
    }

    public override void Apply()
    {
        base.Apply();
        victim.GridObject.CellPosition = to;
    }

    public override float ValueFor(PawnEntity pawn)
    {
        return pawn.Brain.EvaluatePosition(to) - pawn.Brain.EvaluatePosition(from);
    }

    public override void Describe(StringBuilder sb, bool result)
    {
        if (from == to)
            return;
        base.Describe(sb, result);
        sb.Append("´Ó");
        sb.Append(from);
        sb.Append("ÒÆ¶¯µ½");
        sb.Append(to);
        sb.AppendLine();
    }
}
