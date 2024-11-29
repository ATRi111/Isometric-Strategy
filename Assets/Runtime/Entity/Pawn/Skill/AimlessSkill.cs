using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 无需选择目标的技能
/// </summary>
[CreateAssetMenu(fileName = "无目标技能", menuName = "技能/无目标技能",order = -1)]
public class AimlessSkill : Skill
{
    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        ret.Add(position);
    }

    public override void GetVictims(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Entity> ret)
    {
        base.GetVictims(agent, igm, position, target, ret);
        ret.Add(agent);
    }
}