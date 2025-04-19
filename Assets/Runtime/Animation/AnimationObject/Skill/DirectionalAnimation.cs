using MyTool;
using UnityEngine;

/// <summary>
/// �ڼ����ͷ�λ������һ�����岢���ݼ��ܷ�����ת�Ķ���
/// </summary>
public class DirectionalAnimation : AnimationObject
{
    private static Vector3 CenterOffset = new(0.5f, 0.5f, 1.5f);

    public override void Initialize(IAnimationSource source)
    {
        base.Initialize(source);
        PawnAction action = source as PawnAction;
        Vector3Int cellDirection = action.target - action.position;
        Vector2 direction = igm.CellToWorld(cellDirection);
        float angle = direction.ToAngle();
        transform.eulerAngles = new Vector3(0, 0, angle);
        transform.position += igm.CellToWorld(CenterOffset);
    }
}
