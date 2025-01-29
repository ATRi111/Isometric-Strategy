using AStar;
using EditorExtend.GridEditor;
using UnityEngine;

public class ANode : Node
{
    protected IsometricGridManager igm;
    public GridObject gridObject;
    public Entity entity;
    public int aboveGroundLayer;
    public Vector3Int cellPosition;
    public GridSurface surface;

    protected bool isObstacle;
    public override bool IsObstacle => isObstacle;
    public bool isEntity;

    public float difficulty;    //移动力的影响体现在AMover中(因为可能有AMover无视移动难度)

    public ANode(PathFindingProcess process, Vector2Int position, IsometricGridManager igm) : 
        base(process, position)
    {
        this.igm = igm;
        gridObject = igm.GetObjectXY(position);
        aboveGroundLayer = igm.AboveGroundLayer(Position);
        cellPosition =  Position.AddZ(aboveGroundLayer);
        isObstacle = gridObject == null;     //只有不存在地面这一种情况是绝对的障碍物
        if(gridObject != null)
            entity = gridObject.GetComponent<Entity>();
        isEntity = entity != null;
        difficulty = 1;
        if (gridObject != null && gridObject.TryGetComponent(out surface))
        {
            difficulty = surface.difficulty;
        }
    }
}
