using UnityEngine;

public class AnimationProcess_Move : AnimationProcess
{
    private readonly GridMoveController moveController;
    private MoveEffect Effect => effect as MoveEffect;

    // 移动速度倍率（规定行走为1）
    public float speedMultiplier;

    public AnimationProcess_Move(Effect effect, float speedMultiplier)
        :base(effect)
    {
        moveController = effect.victim.MoveController;
        this.speedMultiplier = speedMultiplier;
    }

    public override void Play()
    {
        base.Play();
        moveController.AfterMove += AfterMove;
        moveController.SetGridRoute(Effect.route, speedMultiplier * moveController.defaultSpeed);
    }

    private void AfterMove()
    {
        manager.Unregister(this);
        moveController.AfterMove -= AfterMove;
    }
}
