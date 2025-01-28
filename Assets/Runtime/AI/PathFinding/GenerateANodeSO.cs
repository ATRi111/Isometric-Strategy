using AStar;
using UnityEngine;

[CreateAssetMenu(fileName = "����ANode", menuName = "AStar/�����½ڵ�ķ���/����ANode")]
public class GenerateANodeSO : GenerateNodeSO
{
    public IsometricGridManager Igm => IsometricGridManager.Instance;

    public override Node GenerateNode(PathFindingProcess process, Vector2Int position)
    {
        return new ANode(process, position, Igm);
    }
}
