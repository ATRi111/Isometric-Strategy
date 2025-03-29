using EditorExtend.GridEditor;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// ��һ����Χ���ͷŵļ��ܣ�Ĭ�����÷�ΧΪ����
/// </summary>
[CreateAssetMenu(fileName = "���弼��", menuName = "����/���弼��", order = -1)]
public class RangedSkill : AimSkill
{
    public int castingDistance = 1;
    public bool aimAtSelf;

    /// <summary>
    /// ��ȡ��ѡʩ��λ��
    /// </summary>
    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        List<Vector2Int> primitive = IsometricGridUtility.WithinProjectManhattanDistance(castingDistance);
        if (aimAtSelf)
            primitive.Add(Vector2Int.zero);
        for (int i = 0; i < primitive.Count; i++)
        {
            Vector3Int option = igm.AboveGroundPosition((Vector2Int)position + primitive[i]);
            if (FilterOption(agent, igm, position, option))
                ret.Add(option);
        }
    }

    /// <summary>
    /// �ж�ĳ��λ���Ƿ��ǿ�ѡ���ͷŷ�Χ
    /// </summary>
    public virtual bool FilterOption(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int option)
    {
        if (!igm.Contains((Vector2Int)option))    //�ж��Ƿ��ڵ�ͼ��
            return false;
        if (!LayerCheck(position, option))  //�ж��ͷ�λ�ø߶Ȳ��Ƿ����
            return false;
        return true;
    }
    
    public override void MockArea(IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Vector3Int> ret)
    {
        ret.Clear();
        if (igm.Contains((Vector2Int)target))
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
        sb.Append("ʩ�Ÿ߶Ȳ�:");
        sb.Append(minLayer);
        sb.Append("~");
        sb.Append(maxLayer.ToString("+0"));
        sb.AppendLine();
    }

    protected virtual void DescribeArea(StringBuilder sb)
    {
        sb.Append("���弼��");
        sb.AppendLine();
    }
}
