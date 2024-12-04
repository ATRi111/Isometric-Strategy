using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

public enum EVictimType
{
    Everything = 0,
    Entity = 1,
    Pawn = 2,
}

/// <summary>
/// 在一定范围内释放的技能
/// </summary>
[CreateAssetMenu(fileName = "范围型技能", menuName = "技能/范围型技能", order = -1)]
public class RangedSkill : AimSkill
{
    public int castingDistance;
    public EVictimType victimType;

    /// <summary>
    /// 获取可选施放位置
    /// </summary>
    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        List<Vector2Int> primitive = IsometricGridUtility.WithinProjectManhattanDistance(castingDistance);
        List<Entity> victims = new();
        for (int i = 0; i < primitive.Count; i++)
        {
            Vector2Int temp = (Vector2Int)position + primitive[i];
            Vector3Int target = temp.AddZ(igm.AboveGroundLayer(temp));
            switch(victimType)
            {
                case EVictimType.Everything:
                    if(igm.ObjectDict.ContainsKey(target + Vector3Int.back))
                        ret.Add(target);
                    break;
                case EVictimType.Entity:
                case EVictimType.Pawn:
                    GetVictims(agent, igm, position, target, victims);
                    if (victims.Count > 0)
                        ret.Add(target);
                    break;
            }
        }
    }

    /// <summary>
    /// 判断某个Entity是否能作为技能的目标
    /// </summary>
    public override bool FilterVictim(Entity entity)
    {
        return victimType  switch
        {
            EVictimType.Everything or EVictimType.Entity => true,
            EVictimType.Pawn => entity is PawnEntity,
            _ => false,
        };
}

    /// <summary>
    /// 获取技能将会命中的目标
    /// </summary>
    public override void GetVictims(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Entity> ret)
    {
        base.GetVictims(agent, igm, position, target, ret);
        if (igm.EntityDict.ContainsKey(target) && FilterVictim(igm.EntityDict[target]))
            ret.Add(igm.EntityDict[target]);
    }
}
