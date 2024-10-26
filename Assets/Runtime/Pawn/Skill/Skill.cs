using UnityEngine;

public class Skill : ScriptableObject
{
    public int actionTime;

    public virtual void Mock(PawnEntity agent, Vector2Int target,IsometricGridManager igm, EffectUnit ret)
    {
        ret.timeEffect.current += actionTime;
    }
}
