/// <summary>
/// ʹ����Ŀ�궶��
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
