using System.Collections.Generic;
using UnityEngine;

public class RangedSkill : Skill
{
    public int castingDistance;

    public override void GetOptions(IsometricGridManager igm, Vector2Int position, List<Vector2Int> ret)
    {
        base.GetOptions(igm, position, ret);
        //TODO:¼¼ÄÜ·¶Î§
    }

    public virtual bool IsAvailable(IsometricGridManager igm, Vector2Int target)
    {
        return true;
    }
}
