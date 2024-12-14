using AStar;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "�����ƶ�(������Ծ)", menuName = "AStar/��ȡ���ڽڵ�ķ���/�����ƶ�(������Ծ)")]
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
