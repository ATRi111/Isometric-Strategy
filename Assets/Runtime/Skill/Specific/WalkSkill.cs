using MyTool;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "移动", menuName = "技能/特殊/移动")]
public class WalkSkill : MoveSkill
{
    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        agent.Sensor.FindAvailable((Vector2Int)position, ret);
        ret.Remove(position);
    }

    private readonly List<Vector3Int> route = new();
    public override void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        agent.Sensor.FindRoute((Vector2Int)position, (Vector2Int)target, route);
        base.Mock(agent, igm, position, target, ret);
        List<Vector3> temp = new();
        if(route.Count > 0)
        {
            temp.Add(route[0]);
            for (int i = 1; i < route.Count; i++)
            {
                Vector3 next = route[i];
                Vector3 prev = temp[^1];
                if (next.z > prev.z)        //上移时轨迹
                    temp.Add(prev.ResetZ(next.z) + 0.02f * (next - prev).ResetZ());
                else if (next.z < prev.z)   //下移时轨迹
                    temp.Add(next.ResetZ(prev.z) + 0.02f * (next - prev).ResetZ());
                temp.Add(next);
            }
        }
        ret.effects.Add(new MoveEffect(agent, position, target, temp));
    }

    protected override void DescribeTime(StringBuilder sb)
    {
        sb.Append("时间消耗:");
        if(actionTime > 0)
        {
            sb.Append(actionTime);
            sb.Append("+");
        }
        sb.Append(actionTimePerUnit);
        sb.Append("×走过的格数");
        if(ZOCActionTime > 0)
        {
            sb.Append("(试图离开控制区时+");
            sb.Append(ZOCActionTime);
            sb.Append(")");
        }
        else
        {
            sb.Append("(无视控制区)");
        }
        sb.AppendLine();
    }
}
