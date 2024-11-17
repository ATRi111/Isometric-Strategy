using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

public enum ETargetType
{
    Everything = 0,
    Entity = 1,
    Pawn = 2,
}

/// <summary>
/// 在一定范围内释放的技能
/// </summary>
public class RangedSkill : Skill
{
    public int castingDistance;
    public ETargetType targetType;

    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        List<Vector2Int> primitive = IsometricGridUtility.WithinProjectManhattanDistance(castingDistance);
        for (int i = 0; i < primitive.Count; i++)
        {
            Vector2Int temp = (Vector2Int)position + primitive[i];
            Vector3Int target = temp.AddZ(igm.AboveGroundLayer(temp));
            if (TargetCheck(igm, target))
                ret.Add(target);
        }
    }

    public virtual bool TargetCheck(IsometricGridManager igm, Vector3Int target)
    {
        if(!igm.MaxLayerDict.ContainsKey((Vector2Int)target)) 
            return false;

        switch(targetType)
        {
            case ETargetType.Everything:
                return true;
            case ETargetType.Entity:
                return igm.EntityDict.ContainsKey(target);
            case ETargetType.Pawn:
                if (!igm.EntityDict.ContainsKey(target))
                    return false;
                Entity entity = igm.EntityDict[target];
                return entity is PawnEntity;
            default:
                return false;
        }
    }
}
