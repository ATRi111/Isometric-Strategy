using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "移动", menuName = "技能/移动")]
public class MoveSkill : Skill
{
    public static Vector3Int[] ZoneOfControl = 
    {
        Vector3Int.left,
        Vector3Int.left + Vector3Int.forward,
        Vector3Int.left - Vector3Int.forward,
        Vector3Int.right,
        Vector3Int.right + Vector3Int.forward,
        Vector3Int.right - Vector3Int.forward,
        Vector3Int.up,
        Vector3Int.up + Vector3Int.forward,
        Vector3Int.up - Vector3Int.forward,
        Vector3Int.down,
        Vector3Int.down + Vector3Int.forward,
        Vector3Int.down - Vector3Int.forward,
    };

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
        int time = 0;
        for (int i = 0; i < ZoneOfControl.Length; i++)
        {
            Vector3Int temp = position + target;
            if (igm.EntityDict.ContainsKey(temp) && agent.CheckFaction(igm.EntityDict[temp]) == -1)
                time += actionTime;
        }
        return time + DistanceBetween(position, target) * actionTime;
    }
}
