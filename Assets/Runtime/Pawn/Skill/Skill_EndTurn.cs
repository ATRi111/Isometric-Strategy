using UnityEngine;

public class Skill_EndTurn : Skill
{
    public override void Mock(PawnEntity agent, Vector2Int target, IsometricGridManager igm, EffectUnit ret)
    {
        base.Mock(agent, target, igm, ret);
        ret.timeEffect.current += agent.Property.actionTime;
    }
}
