using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

public enum ETargetFlag
{
    None = 0,
    Pawn = 1,
    Destroyable = 2,
}

public class RangedSkill : Skill
{
    public int castingDistance;
    public int targetFlags;

    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector2Int position, List<Vector2Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        List<Vector2Int> primitive = IsometricGridUtility.WithinProjectManhattanDistance(castingDistance);
        for (int i = 0; i < primitive.Count; i++)
        {
            Vector2Int temp = primitive[i] + position;
            if (IsAvailable(igm, temp))
                ret.Add(temp);
        }
    }

    public virtual bool IsAvailable(IsometricGridManager igm, Vector2Int target)
    {
        bool MatchFlag(ETargetFlag targetFlag)
        {
            return (targetFlags & (int)targetFlag) != 0;
        }

        GridObject gridObject = igm.GetObejectXY(target);
        if(targetFlags == 0)
        {
            return true;
        }
        if(MatchFlag(ETargetFlag.Pawn))
        {
            if (gridObject.GetComponentInParent<PawnEntity>() != null)
                return true;
        }
        if(MatchFlag(ETargetFlag.Destroyable))
        {
            if(gridObject.GetComponentInParent<DestroyableEntity>() != null)
                return true;
        }
        return false;
    }
}
