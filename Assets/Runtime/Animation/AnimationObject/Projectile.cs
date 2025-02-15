using System.Collections.Generic;
using UnityEngine;

public class Projectile : AnimationObject
{
    protected GridMoveController moveController;

    public override void Activate(IAnimationSource source)
    {
        PawnAction action = (PawnAction)source;
        ProjectileSkill skill = (ProjectileSkill)action.skill;
        List<Vector3> trajectory = new();
        skill.HitCheck(action.agent, Igm, action.target, trajectory);
        moveController.SetRoute_CellPosition(trajectory, skill.speedMultiplier * moveController.defaultSpeed);
        lifeSpan = moveController.MockTime_CellPosition(trajectory, skill.speedMultiplier * moveController.defaultSpeed);
        transform.position = trajectory[0];
        StartCoroutine(DelayRecycle(lifeSpan));
    }

    public override float CalculateLifeSpan(IAnimationSource source)
    {
        PawnAction action = (PawnAction)source;
        ProjectileSkill skill = (ProjectileSkill)action.skill;
        List<Vector3> trajectory = new();
        skill.HitCheck(action.agent, Igm, action.target, trajectory);
        return moveController.MockTime_CellPosition(trajectory, skill.speedMultiplier * moveController.defaultSpeed);
    }

    protected override void Awake()
    {
        base.Awake();
        moveController = GetComponent<GridMoveController>();
    }
}
