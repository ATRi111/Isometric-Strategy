using AStar;
using UnityEngine;

[CreateAssetMenu(fileName = "生成ANode", menuName = "AStar/生成新节点的方法/生成ANode")]
public class GenerateANodeSO : GenerateNodeSO
{
    protected IsometricGridManager igm;
    public IsometricGridManager Igm
    {
        get
        {
            if (igm == null)
                igm = IsometricGridManager.FindInstance();
            return igm;
        }
    }

    public override Node GenerateNode(PathFindingProcess process, Vector2Int position)
    {
        //TODO:困难地形
        return new ANode(process, position, Igm, 1f);
    }
}
