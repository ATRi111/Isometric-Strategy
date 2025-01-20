using EditorExtend.GridEditor;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// ��һ����Χ���ͷŵļ��ܣ�Ĭ�����÷�ΧΪ����
/// </summary>
[CreateAssetMenu(fileName = "���弼��", menuName = "����/���弼��", order = -1)]
public class RangedSkill : AimSkill
{
    public int castingDistance = 1;

    /// <summary>
    /// ��ȡ��ѡʩ��λ��
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
        sb.Append("ʩ�ŷ�Χ:");
        sb.Append(castingDistance);
        sb.AppendLine();
    }

    protected virtual void DescribeArea(StringBuilder sb)
    {
        sb.Append("���弼��");
        sb.AppendLine();
    }
}
