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

        GridObject gridObject = igm.GetObject(target);
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
