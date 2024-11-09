using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ÒÆ¶¯", menuName = "Skill/ÒÆ¶¯")]
public class Skill_Move : Skill
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
        ret.effects.Add(new Effect_Move(agent, route));
    }

    public override int MockTime(PawnEntity agent, Vector3Int position, Vector3Int target, IsometricGridManager igm)
    {
        return DistanceBetween(position, target) * actionTime;
    }
}
