using MyTool;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAnimation : AnimationObject
{
    protected GridMoveController moveController;
    protected Vector3 prevPosition;

    public override void Initialize(IAnimationSource source)
    {
        PawnAction action = (PawnAction)source;
        ProjectileSkill skill = (ProjectileSkill)action.skill;
        List<Vector3> trajectory = new();
        skill.HitCheck(action.agent, igm, action.target, trajectory);
        moveController.SetRoute_CellPosition(trajectory, skill.speedMultiplier * moveController.defaultSpeed);
        lifeSpan = moveController.MockTime_CellPosition(trajectory, skill.speedMultiplier * moveController.defaultSpeed);
        transform.position = trajectory[0];
        prevPosition = moveController.ufm.Origin;
        base.Initialize(source);
    }

    public override float GetAnimationLatency(IAnimationSource source)
    {
        PawnAction action = (PawnAction)source;
        ProjectileSkill skill = (ProjectileSkill)action.skill;
        List<Vector3> trajectory = new();
        skill.HitCheck(action.agent, igm, action.target, trajectory);
        return nomalizedLatency * moveController.MockTime_CellPosition(trajectory, skill.speedMultiplier * moveController.defaultSpeed);
    }

    private void Ontick(Vector3 position)
    {
        Vector2 delta = position - prevPosition;
        float angle = delta.ToAngle();
        transform.eulerAngles = new Vector3(0, 0, angle);
        prevPosition = position;
    }

    protected override void Awake()
    {
        base.Awake();
        moveController = GetComponent<GridMoveController>();
        moveController.ufm.OnTick += Ontick;
    }

}
