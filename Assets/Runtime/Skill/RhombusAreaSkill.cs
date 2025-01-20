using EditorExtend.GridEditor;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "���η�Χ����", menuName = "����/���η�Χ����", order = -1)]
public class RhombusAreaSkill : RangedSkill
{
    public int effectRange = 1;

    public override void MockArea(IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Vector3Int> ret)
    {
        ret.Clear();
        List<Vector2Int> primitive = IsometricGridUtility.WithinProjectManhattanDistance(effectRange);
        for (int i = 0; i < primitive.Count; i++)
        {
            Vector2Int xy = (Vector2Int)target + primitive[i];
            ret.Add(igm.AboveGroundPosition(xy));
        }
    }

    protected override void DescribeArea(StringBuilder sb)
    {
        sb.Append("���÷�Χ:");
        sb.Append(effectRange);
        sb.AppendLine();
    }
}
