using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_Move", menuName = "Skill/Move")]
public class Skill_Move : Skill
{
    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector2Int position, List<Vector2Int> ret)
    {
        agent.Brain.FindAvalable(position, ret);
    }

    private readonly List<Vector3Int> route = new();
    public override void Mock(PawnEntity agent, IsometricGridManager igm, Vector2Int position, Vector2Int target, EffectUnit ret)
    {
        base.Mock(agent, igm, position, target, ret);
        agent.Brain.FindRoute(position, target, route);
        ret.effects.Add(new Effect_Move(agent, route));
    }

    public override int MockTime(PawnEntity agent, Vector2Int position, Vector2Int target, IsometricGridManager igm)
    {
        return DistanceBetween(position, target) * actionTime;
    }
}
