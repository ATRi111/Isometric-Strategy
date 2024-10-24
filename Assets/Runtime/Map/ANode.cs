using AStar;
using EditorExtend.GridEditor;
using UnityEngine;

public class ANode : AStarNode
{
    protected IsometricGridManager igm;

    public GridObject CurrentObject => igm.GetObejectXY(position);
    public virtual int AboveGroundLayer => igm.AboveGroundLayer(Position);
    public Vector3Int CellPositon => Position.AddZ(AboveGroundLayer);

    public override bool IsObstacle
    {
        get
        {
            int layer = igm.AboveGroundLayer(position);
            if (layer == 0)
                return true;
            return AboveGroundLayer >= GridUtility.MaxHeight;
        }
    }

    public float difficulty;

    public ANode(PathFindingProcess process, Vector2Int position, IsometricGridManager igm, float difficulty) : 
        base(process, position)
    {
        this.igm = igm;
        this.difficulty = difficulty;
    }
}
