using AStar;
using EditorExtend.GridEditor;
using UnityEngine;

public class ANode : Node
{
    protected IsometricGridManager igm;
    public GridObject gridObject;
    public MovableGridObject movableGridObject;
    public int aboveGroundLayer;
    public Vector3Int cellPosition;

    protected bool isObstacle;
    public override bool IsObstacle => isObstacle;
    public bool isPawn;

    public float difficulty;

    public ANode(PathFindingProcess process, Vector2Int position, IsometricGridManager igm, float difficulty) : 
        base(process, position)
    {
        this.igm = igm;
        this.difficulty = difficulty;
        gridObject = igm.GetObjectXY(position);
        aboveGroundLayer = igm.AboveGroundLayer(Position);
        cellPosition =  Position.AddZ(aboveGroundLayer);
        isObstacle = gridObject == null;     //只有不存在地面这一种情况是绝对的障碍物
        movableGridObject = gridObject as MovableGridObject;
        isPawn = movableGridObject != null;
    }
}
