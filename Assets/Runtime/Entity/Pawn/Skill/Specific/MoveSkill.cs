using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "移动", menuName = "技能/移动")]
public class MoveSkill : Skill
{
    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        agent.Brain.FindAvalable(position, ret);
    }

    private readonly List<Vector3Int> route = new();
    public override void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.Mock(agent, igm, position, target, ret);
        agent.Brain.FindRoute(position, target, route);
        ret.effects.Add(new MoveEffect(agent, route));
    }

    public override int MockTime(PawnEntity agent, Vector3Int position, Vector3Int target, IsometricGridManager igm)
    {
        return DistanceBetween(position, target) * actionTime;
    }
}
