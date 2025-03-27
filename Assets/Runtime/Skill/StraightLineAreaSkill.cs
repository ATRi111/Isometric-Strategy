using MyTool;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "直线范围技能", menuName = "技能/直线范围技能", order = -1)]
public class StraightLineAreaSkill : RangedSkill
{
    public int length = 2;
    public int width = 1;

    public override void MockArea(IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Vector3Int> ret)
    {
        ret.Clear();
        Vector2Int targetXY = (Vector2Int)target;
        Vector2Int front = targetXY - (Vector2Int)position;
        float angle = ((Vector2)front).ToAngle();
        Vector2Int left = EDirectionTool.NearestDirection((angle + 90f).ToDirection());
        Vector2Int right = EDirectionTool.NearestDirection((angle - 90f).ToDirection());
        List<Vector2Int> startPoints = new() { (Vector2Int)target };
        for (int i = 1; i <= width / 2 ; i++)
        {
            startPoints.Add(targetXY + i * left);
            startPoints.Add(targetXY + i * right);
        }

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < startPoints.Count; j++)
            {
                Vector2Int xy = startPoints[j] + front * j;
                Vector3Int p = igm.AboveGroundPosition(xy);
                if (!LayerCheck(position, p))
                    continue;
                if (igm.Contains(xy))
                    ret.Add(igm.AboveGroundPosition(xy));
            }
        }
    }

    protected override void DescribeArea(StringBuilder sb)
    {
        sb.Append("作用范围:");
        sb.Append("长度");
        sb.Append(length);
        sb.Append("宽度");
        sb.Append(width);
        sb.Append("的直线");
        sb.AppendLine();
    }
}
