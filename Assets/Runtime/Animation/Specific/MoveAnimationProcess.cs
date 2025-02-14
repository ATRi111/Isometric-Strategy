public class MoveAnimationProcess : EffectAnimationProcess
{
    private readonly GridMoveController moveController;

    // 移动速度倍率（规定行走为1）
    public readonly float speedMultiplier;

    public MoveAnimationProcess(MoveEffect effect)
        :base(effect)
    {
        moveController = effect.victim.MoveController;
        speedMultiplier = effect.speedMultiplier;
    }

    public override float MockTime()
    {
        return moveController.MockTime((effect as MoveEffect).route, speedMultiplier * moveController.defaultSpeed);
    }

    public override void Play()
    {
        moveController.AfterMove += OneOffComplete;
        moveController.SetGridRoute((effect as MoveEffect).route, speedMultiplier * moveController.defaultSpeed);
    }

    private void OneOffComplete()
    {
        Complete();
        moveController.AfterMove -= OneOffComplete;
    }
}
