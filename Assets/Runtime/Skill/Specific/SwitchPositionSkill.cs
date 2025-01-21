using UnityEngine;

[CreateAssetMenu(fileName = "位置互换", menuName = "技能/特殊/位置互换")]
public class SwitchPositionSkill : RangedSkill
{
    private static Vector3Int BeyondMapOffset = new(114, 514, 0);
    
    protected override void MockOtherEffectOnVictim(PawnEntity agent, Entity victim, EffectUnit ret)
    {
        base.MockOtherEffectOnVictim(agent, victim, ret);
        PawnEntity pawn = victim as PawnEntity;
        if (pawn != null)
        {
            Vector3Int position_agent = agent.MovableGridObject.CellPosition;
            Vector3Int position_victim = pawn.MovableGridObject.CellPosition;
            TeleportEffect removeAgent = new(agent, position_agent, position_agent + BeyondMapOffset);
            TeleportEffect moveVictim = new(pawn, position_victim, position_agent);
            TeleportEffect moveAgent = new(agent, position_agent + BeyondMapOffset, position_victim);
            ret.effects.Add(removeAgent);
            ret.effects.Add(moveVictim);
            ret.effects.Add(moveAgent);
        }
    }
}
