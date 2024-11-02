using System.Collections.Generic;
using UnityEngine;

public class NoRangeSkill : Skill
{
    public override void GetOptions(IsometricGridManager igm, Vector2Int position, List<Vector2Int> ret)
    {
        base.GetOptions(igm, position, ret);
        ret.Add(position);
    }
}
