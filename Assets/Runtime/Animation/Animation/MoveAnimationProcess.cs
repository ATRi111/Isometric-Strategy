using UnityEngine;

public class MoveAnimationProcess : EffectAnimationProcess
{
    private readonly GridObjectMoveController moveController;
    private Animator animator;

    // 移动速度倍率（规定行走为1）
    public readonly float speedMultiplier;

    public MoveAnimationProcess(MoveEffect effect)
        :base(effect)
    {
        moveController = effect.victim.MoveController;
        speedMultiplier = effect.speedMultiplier;
        animator = effect.victim.GetComponentInChildren<Animator>();
    }

    public override float MockLatency(IAnimationSource source)
    {
        return moveController.MockTime_CellPosition((effect as MoveEffect).route, speedMultiplier * moveController.defaultSpeed);
    }

    public override void Play()
    {
        moveController.AfterMove += OneOffComplete;
        animator.speed = 2f;
        moveController.SetRoute_CellPosition((effect as MoveEffect).route, speedMultiplier * moveController.defaultSpeed);
    }

    private void OneOffComplete()
    {
        Complete();
        animator.speed = 1f;
        moveController.AfterMove -= OneOffComplete;
    }
}
