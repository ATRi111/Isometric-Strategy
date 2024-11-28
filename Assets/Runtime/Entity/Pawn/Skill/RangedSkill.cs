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
[CreateAssetMenu(fileName = "范围型技能", menuName = "技能/范围型技能")]
public class RangedSkill : Skill
{
    public int castingDistance;
    public ETargetType targetType;

    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        List<Vector2Int> primitive = IsometricGridUtility.WithinProjectManhattanDistance(castingDistance);
        List<Entity> victims = new();
        for (int i = 0; i < primitive.Count; i++)
        {
            Vector2Int temp = (Vector2Int)position + primitive[i];
            Vector3Int target = temp.AddZ(igm.AboveGroundLayer(temp));
            GetVictims(agent, igm, position, target, victims);
            if (victims.Count > 0)
                ret.Add(target);
        }
    }

    public override void GetVictims(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Entity> ret)
    {
        base.GetVictims(agent, igm, position, target, ret);
        //TODO:AOE技能
        switch (targetType)
        {
            case ETargetType.Everything:
            case ETargetType.Entity:
                if (igm.EntityDict.ContainsKey(target))
                    ret.Add(igm.EntityDict[target]);
                break;
            case ETargetType.Pawn:
                if (igm.EntityDict.ContainsKey(target) && igm.EntityDict[target] is PawnEntity pawn)
                    ret.Add(pawn);
                break;
        }
    }
}
