using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 无需选择目标的技能
/// </summary>
public class AimlessSkill : Skill
{
    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        ret.Add(position);
    }
}
