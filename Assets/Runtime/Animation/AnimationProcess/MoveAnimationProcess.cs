public class MoveAnimationProcess : EffectAnimationProcess
{
    private readonly GridObjectMoveController moveController;

    // 移动速度倍率（规定行走为1）
    public readonly float speedMultiplier;

    public MoveAnimationProcess(MoveEffect effect)
        :base(effect)
    {
        moveController = effect.victim.MoveController;
        speedMultiplier = effect.speedMultiplier;
    }

    public override float MockLatency(IAnimationSource source)
    {
        return moveController.MockTime_CellPosition((effect as MoveEffect).route, speedMultiplier * moveController.defaultSpeed);
    }

    public override void Play()
    {
        moveController.AfterMove += OneOffComplete;
        moveController.SetRoute_CellPosition((effect as MoveEffect).route, speedMultiplier * moveController.defaultSpeed);
    }

    private void OneOffComplete()
    {
        Complete();
        moveController.AfterMove -= OneOffComplete;
    }
}
