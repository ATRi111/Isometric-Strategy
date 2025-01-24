using AStar;

/// <summary>
/// ���ڲ�࣬�ƶ���ͣ��ʱ����Pawn����Ծʱ�����ѷ�Pawn
/// </summary>
public class AMover_Ranging : AMover
{
    public AMover_Ranging(MovableGridObject gridObject) 
        : base(gridObject)
    {
    }

    public override bool MoveCheck(Node from, Node to)
    {
        if ((from.Position - to.Position).sqrMagnitude == 1)
            return base.MoveCheck(from, to);

        return gridObject.JumpCheck(from.Position, to.Position, MovableGridObject.ObjectCheck_IgnoreAlly);
    }

    public override bool MoveAbilityCheck(Node node)
    {
        return true;    //���ʱ���迼���ƶ�������
    }
}
