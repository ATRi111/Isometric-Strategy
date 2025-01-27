using AStar;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "�����ƶ�(������Ծ)", menuName = "AStar/��ȡ���ڿɴ�ڵ�ķ���/�����ƶ�(������Ծ)")]
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
                to = process.GetNode(from.Position + jumpDistance * direction); //���ڸ񲻿ɽ���ʱ���Żῼ����Ծ
                if (moveCheck(from, to))
                    ret.Add(to);
            }
        }
    }
}
