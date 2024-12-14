using AStar;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "四向移动(考虑跳跃)", menuName = "AStar/获取相邻节点的方法/四向移动(考虑跳跃)")]
public class GetFourWithJumpSO : GetAdjoinedNodesSO
{
    public int jumpDistance = 2;

    public override void GetAdjoinedNodes(PathFindingProcess process, Node node, List<Node> ret)
    {
        PathFindingUtility.GetAdjoinNodes_Four(process, node, ret); 
        foreach (Vector2Int direction in PathFindingUtility.FourDirections)
        {
            ret.Add(process.GetNode(node.Position + jumpDistance * direction));
        }
    }
}
