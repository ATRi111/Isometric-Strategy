using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 在技能作用范围内各位置生成同样物体的动画
/// </summary>
public class AreaAnimation : AnimationObject
{
    public string prefabName;
    private readonly List<Vector3Int> area = new();

    public override void Initialize(IAnimationSource source)
    {
        base.Initialize(source);
        if(!string.IsNullOrWhiteSpace(prefabName))
        {
            PawnAction action = source as PawnAction;
            AimSkill skill = action.skill as AimSkill;
            skill.MockArea(igm, action.position, action.target, area);
            for (int i = 0; i < area.Count; i++)
            {
                Vector3 world = igm.CellToWorld(area[i]);
                objectManager.Activate(prefabName, world, Vector3.zero, transform);
            }
        }
    }
}
