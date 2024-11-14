using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

public enum ETargetFlag
{
    None = 0,
    Entity = 1,
    Pawn = 2,
}

/// <summary>
/// 无弹道，直接命中所选位置的技能
/// </summary>
public class RangedSkill : Skill
{
    public int castingDistance;
    public int targetFlags;

    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        List<Vector2Int> primitive = IsometricGridUtility.WithinProjectManhattanDistance(castingDistance);
        for (int i = 0; i < primitive.Count; i++)
        {
            Vector2Int temp = (Vector2Int)position + primitive[i];
            Vector3Int target = temp.AddZ(igm.AboveGroundLayer(temp));
            if (IsAvailable(igm, target))
                ret.Add(target);
        }
    }

    public virtual bool IsAvailable(IsometricGridManager igm, Vector3Int target)
    {
        bool MatchFlag(ETargetFlag targetFlag)
        {
            return (targetFlags & (int)targetFlag) != 0;
        }

        if(MatchFlag(ETargetFlag.Entity))
        {
            return igm.EntityDict.ContainsKey(target);
        }
        if (MatchFlag(ETargetFlag.Pawn))
        {
            if (!igm.EntityDict.ContainsKey(target))
                return false;
            Entity entity = igm.EntityDict[target];
            return entity is PawnEntity;
        }
        return true;
    }
}
