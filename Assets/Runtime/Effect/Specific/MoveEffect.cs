using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MoveEffect : TeleportEffect
{
    public readonly List<Vector3> route;
    // 移动速度倍率（规定行走为1）
    public float speedMultiplier;

    public MoveEffect(PawnEntity victim, Vector3Int from, Vector3Int to, List<Vector3> route, float speedMultiplier = 1f, int probability = MaxProbability)
        : base(victim, from, to, probability)
    {
        this.route = new();
        this.route.AddRange(route);
        this.speedMultiplier = speedMultiplier;
    }

    public override AnimationProcess GenerateAnimation()
    {
        return new AnimationProcess_Move(this, speedMultiplier);
    }
}
