/// <summary>
/// 使命中目标抖动
/// </summary>
public class HitGenerator : AnimationObject
{
    public override void Initialize(IAnimationSource source)
    {
        PawnAction action = source as PawnAction;
        HitUtility.GenerateHit(action);
        base.Initialize(source);
    }

    public override float GetAnimationLatency(IAnimationSource source)
    {
        return 0f;
    }
}
