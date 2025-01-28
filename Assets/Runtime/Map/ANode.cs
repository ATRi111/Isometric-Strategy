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
    public GridGround gridGround;

    protected bool isObstacle;
    public override bool IsObstacle => isObstacle;
    public bool isPawn;

    public float difficulty;    //�ƶ�����Ӱ��������AMover��(��Ϊ������AMover�����ƶ��Ѷ�)

    public ANode(PathFindingProcess process, Vector2Int position, IsometricGridManager igm) : 
        base(process, position)
    {
        this.igm = igm;
        gridObject = igm.GetObjectXY(position);
        aboveGroundLayer = igm.AboveGroundLayer(Position);
        cellPosition =  Position.AddZ(aboveGroundLayer);
        isObstacle = gridObject == null;     //ֻ�в����ڵ�����һ������Ǿ��Ե��ϰ���
        movableGridObject = gridObject as MovableGridObject;
        isPawn = movableGridObject != null;
        difficulty = 1;
        if (gridObject != null && gridObject.TryGetComponent(out gridGround))
        {
            difficulty = gridGround.difficulty;
        }
    }
}
