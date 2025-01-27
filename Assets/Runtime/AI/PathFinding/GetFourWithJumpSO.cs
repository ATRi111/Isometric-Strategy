using AStar;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "四向移动(考虑跳跃)", menuName = "AStar/获取相邻可达节点的方法/四向移动(考虑跳跃)")]
public class GetFourWithJumpSO : GetMovableNodesSO
{
    public int jumpDistance = 2;

    public override void GetMovableNodes(PathFindingProcess process, Node from, Func<Node, Node, bool> moveCheck, List<Node> ret)
    {
        ret.Clear();
        Node to;
        foreach (Vector2Int direction in PathFindingUtility.FourDirections)
        {
            to = process.GetNode(from.Position + direction);
            if (moveCheck(from, to))
            {
                ret.Add(to);
            }
            else
            {
                to = process.GetNode(from.Position + jumpDistance * direction); //相邻格不可进入时，才会考虑跳跃
                if (moveCheck(from, to))
                    ret.Add(to);
            }
        }
    }
}
