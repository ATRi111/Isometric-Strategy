using System.Collections.Generic;
using UnityEngine;

public class NoRangeSkill : Skill
{
    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector2Int position, List<Vector2Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        ret.Add(position);
    }
}
