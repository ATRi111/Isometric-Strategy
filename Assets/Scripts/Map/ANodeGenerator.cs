using AStar;
using UnityEngine;

public class ANodeGenerator : MonoBehaviour
{
    private IsometricGridManager igm;
    private PathFindingProcess process;

    private void Awake()
    {
        igm = IsometricGridManager.FindInstance();
    }

    public AStarNode GenerateNode(PathFindingProcess process, Vector2Int position)
    {
        return new ANode(process, position, igm);
    }
}
