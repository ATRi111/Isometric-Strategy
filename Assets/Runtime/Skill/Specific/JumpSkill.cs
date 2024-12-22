using EditorExtend.GridEditor;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "跳跃", menuName = "技能/跳跃")]
public class JumpSkill : MoveSkill
{
    private readonly static List<Vector2Int> Directions = new()
    {
        Vector2Int.left,
        Vector2Int.right,
        Vector2Int.up,
        Vector2Int.down,
    };
    private static float Gravity => GridPhysics.settings.gravity;

    public int castingDistance;
    public float jumpAngle;

    /// <summary>
    /// 假设agent位于fromXY,判断其能否跳到toXY（ObjectCheck表示跳跃过程中要考虑与哪些物体的碰撞）
    /// </summary>
    public bool JumpCheck(PawnEntity agent, IsometricGridManager igm, Vector2Int fromXY, Vector2Int toXY, Func<PawnEntity, GridObject, bool> ObjectCheck = null)
    {
        GridObject aimedObject = igm.GetObjectXY(toXY);
        if (aimedObject == null)
            return false;
        if (!ObjectCheck.Invoke(agent, aimedObject))
            return false;

        Vector3Int position = fromXY.AddZ(igm.AboveGroundLayer(fromXY));
        Vector3 from = agent.GridObject.BottomCenter - agent.MovableGridObject.CellPosition + position;
        Vector3 to = aimedObject.TopCenter;
        if (!GridPhysics.InitialVelocityOfParabola(from, to, jumpAngle, Gravity, out Vector3 velocity, out float _))
            return false;

        GridObject gridObject = igm.ParabolaCast(from, velocity, Gravity, agent, ObjectCheck);
        if (gridObject != aimedObject)
            return false;

        return true;
    }

    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        ret.Clear();
        for (int i = 0; i < Directions.Count; i++)
        {
            Vector2Int fromXY = (Vector2Int)position;
            Vector2Int toXY = fromXY + castingDistance * Directions[i];
            if (JumpCheck(agent, igm, fromXY, toXY, MovableGridObject.ObjectCheck_AllObject))
            {
                Vector3Int target = toXY.AddZ(igm.AboveGroundLayer(toXY));
                ret.Add(target);
            }
        }
    }

    public override void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.Mock(agent, igm, position, target, ret);
        List<Vector3> route = new();
        GridPhysics.InitialVelocityOfParabola(position, target, jumpAngle, Gravity, out Vector3 velocity, out float time);
        GridPhysics.DiscretizeParabola(position, velocity, Gravity, time, GridPhysics.settings.parabolaPrecision, route);
        ret.effects.Add(new MoveEffect(agent, position, target, route, speedMultiplier));
    }

    protected override void DescribeTime(StringBuilder sb)
    {
        sb.Append("基础时间消耗:");
        sb.Append(actionTimePerUnit * castingDistance + actionTime);
        sb.Append("(试图从敌人旁离开时,时间消耗增加");
        sb.Append(ZOCActionTime);
        sb.Append(")");
        sb.AppendLine();
    }
}
