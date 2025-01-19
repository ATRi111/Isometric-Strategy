using MyTool;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "移动", menuName = "技能/移动")]
public class WalkSkill : MoveSkill
{
    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        agent.Brain.FindAvailable(position, ret);
        ret.Remove(position);
    }

    private readonly List<Vector3Int> route = new();
    public override void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.Mock(agent, igm, position, target, ret);
        agent.Brain.FindRoute(position, target, route);
        List<Vector3> temp = new();
        if(route.Count > 0)
        {
            temp.Add(route[0]);
            for (int i = 1; i < route.Count; i++)
            {
                if (route[i].z > temp[^1].z) //上移
                    temp.Add(temp[^1].ResetZ(route[i].z));
                else if (route[i].z < temp[^1].z) // 下移
                    temp.Add(new Vector3Int(route[i].x, route[i].y, Mathf.RoundToInt(temp[^1].z)));
                temp.Add(route[i]);
            }
        }
        ret.effects.Add(new MoveEffect(agent, position, target, temp));
    }

    protected override void DescribeTime(StringBuilder sb)
    {
        sb.Append("时间消耗:");
        sb.Append(actionTimePerUnit);
        sb.Append("×走过的格数");
        sb.Append("(试图从敌人旁离开时+");
        sb.Append(ZOCActionTime);
        sb.Append(")");
        sb.AppendLine();
    }
}
