using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "直线范围技能", menuName = "技能/直线范围技能", order = -1)]
public class StraightLineAreaSkill : RangedSkill
{
    public int length = 2;

    public override void MockArea(IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Vector3Int> ret)
    {
        ret.Clear();
        Vector2Int delta = (Vector2Int)(target - position);
        Vector2Int xy = (Vector2Int)(target);
        for (int i = 0; i < length; i++)
        {
            ret.Add(igm.AboveGroundPosition(xy));
            xy += delta;
        }
    }

    protected override void DescribeArea(StringBuilder sb)
    {
        sb.Append("作用范围:");
        sb.Append(length);
        sb.Append("(直线)");
        sb.AppendLine();
    }
}
