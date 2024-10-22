using AStar;
using EditorExtend.GridEditor;
using UnityEngine;

public class ANode : AStarNode
{
    protected IsometricGridManager igm;
    public virtual int AboveGroundLayer => igm.AboveGroundLayer(Position);
    public override bool IsObstacle => AboveGroundLayer >= GridUtility.MaxHeight;

    public ANode(PathFindingProcess process, Vector2Int position,IsometricGridManager igm) : 
        base(process, position)
    {
        this.igm = igm;
    }
}
