using UnityEngine;

//public class HitBackAnimationProcess : DamageNumberAnimationProcess
//{
//    protected readonly GridMoveController moveController;
//    public readonly float speedMultiplier;

//    public HitBackAnimationProcess(HitBackEffect effect, Transform parent, Vector3 position, int damage) 
//        : base(effect, parent, position, damage)
//    {
//        moveController = effect.victim.MoveController;
//        speedMultiplier = effect.speedMultiplier;
//    }

//    public override void Play()
//    {
//        HitBackEffect hitBackEffect = effect as HitBackEffect;
//        moveController.AfterMove += AfterMove;
//        moveController.SetGridRoute((effect as MoveEffect).route, speedMultiplier * moveController.defaultSpeed);
//    }

//    private void AfterMove()
//    {
//        base.Play();
//        moveController.AfterMove -= AfterMove;
//    }
//}
