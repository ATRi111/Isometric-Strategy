using UnityEngine;

public class AnimationProcess_Move : AnimationProcess
{
    private readonly GridMoveController moveController;
    private Effect_Move Effect => effect as Effect_Move;

    public AnimationProcess_Move(Effect effect)
        :base(effect)
    {
        moveController = effect.victim.MoveController;
    }

    public override void Play()
    {
        base.Play();
        moveController.AfterMove += AfterMove;
        moveController.SetGridRoute(Effect.route);
    }

    private void AfterMove()
    {
        manager.Unregister(this);
        moveController.AfterMove -= AfterMove;
    }
}
