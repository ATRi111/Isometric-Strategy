using EditorExtend.GridEditor;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 在一定范围内释放的技能，默认作用范围为单点
/// </summary>
[CreateAssetMenu(fileName = "单体技能", menuName = "技能/单体技能", order = -1)]
public class RangedSkill : AimSkill
{
    public int castingDistance = 1;

    /// <summary>
    /// 获取可选施放位置
    /// </summary>
    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        List<Vector2Int> primitive = IsometricGridUtility.WithinProjectManhattanDistance(castingDistance);
        for (int i = 0; i < primitive.Count; i++)
        {
            Vector2Int temp = (Vector2Int)position + primitive[i];
            Vector3Int target = igm.AboveGroundPosition(temp);
            if (!LayerCheck(position, target))
                continue;
            //List<Entity> victims = new();
            //switch(victimType)
            //{
            //    case EVictimType.Everything:
            //        if(igm.ObjectDict.ContainsKey(target + Vector3Int.back))
            //            ret.Add(target);
            //        break;
            //    case EVictimType.Entity:
            //    case EVictimType.Pawn:
            //        GetVictims(agent, igm, position, target, victims);
            //        if (victims.Count > 0)
            //            ret.Add(target);
            //        break;
            //}
            ret.Add(target);
        }
    }

    public override void MockArea(IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Vector3Int> ret)
    {
        ret.Clear();
        ret.Add(target);
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        DescribeCastingDistance(sb);
        DescribeArea(sb);
    }

    protected virtual void DescribeCastingDistance(StringBuilder sb)
    {
        sb.Append("施放范围:");
        sb.Append(castingDistance);
        sb.AppendLine();
    }

    protected virtual void DescribeArea(StringBuilder sb)
    {
        sb.Append("单体技能");
        sb.AppendLine();
    }
}
