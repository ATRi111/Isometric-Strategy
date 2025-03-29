using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "直线范围技能", menuName = "技能/直线范围技能", order = -1)]
public class StraightLineAreaSkill : RangedSkill
{
    public int length = 2;
    public int width = 1;

    public override bool FilterOption(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int option)
    {
        Vector3Int delta = option - position;
        if(delta.x * delta.y != 0)  //只能在直线方向上释放
            return false;
        return base.FilterOption(agent, igm, position, option);
    }

    public override void MockArea(IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Vector3Int> ret)
    {
        ret.Clear();
        Vector2Int extend;
        Vector2Int front = (Vector2Int)(target - position);
        if(front == Vector2Int.left ||  front == Vector2Int.right)
            extend = Vector2Int.up;
        else if(front == Vector2Int.up || front == Vector2Int.down)
            extend = Vector2Int.right;
        else
            throw new System.ArgumentException();

        Vector2Int targetXY = (Vector2Int)target;
        List<Vector2Int> startPoints = new() { targetXY };
        for (int i = 1; i <= width / 2 ; i++)
        {
            startPoints.Add(targetXY + i * extend);
            startPoints.Add(targetXY - i * extend);
        }

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < startPoints.Count; j++)
            {
                Vector2Int xy = startPoints[j] + i * front;
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
