using MyTool;
using UnityEngine;

/// <summary>
/// 在技能释放位置生成一个物体并根据技能方向旋转的动画
/// </summary>
public class DirectionalAnimation : AnimationObject
{
    public string prefabName;

    public override void Initialize(IAnimationSource source)
    {
        base.Initialize(source);
        if(!string.IsNullOrWhiteSpace(prefabName))
        {
            PawnAction action = source as PawnAction;
            Vector3Int cellDirection = action.target - action.position;
            Vector2 direction = igm.CellToWorld(cellDirection);
            float angle = direction.ToAngle();
            Vector3 worldPosition = igm.CellToWorld(action.target);
            objectManager.Activate(prefabName, worldPosition, new Vector3(0, 0, angle), transform);
        }
    }
}
