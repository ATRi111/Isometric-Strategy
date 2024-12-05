using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "跳跃", menuName = "技能/跳跃")]
public class JumpSkill : MoveSkill
{
    public int castingDistance;
    public float jumpAngle;

    private readonly static List<Vector2Int> Directions = new()
    {
        Vector2Int.left,
        Vector2Int.right,
        Vector2Int.up,
        Vector2Int.down,
    };
    private static float Gravity => GridPhysics.settings.gravity;

    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        ret.Clear();
        for (int i = 0; i < Directions.Count; i++)
        {
            Vector2Int xy = (Vector2Int)position + castingDistance * Directions[i];
            GridObject aimedObject = igm.GetObjectXY(xy);
            if(aimedObject == null)
                continue;
            if (aimedObject.GetComponent<Entity>() != null)  //不可落在Entity上
                continue;

            Vector3 from = agent.GridObject.BottomCenter;
            Vector3 to = aimedObject.TopCenter;
            if (!GridPhysics.InitialVelocityOfParabola(from, to, jumpAngle, Gravity, out Vector3 velocity, out float _))
                continue;

            GridObject gridObject = igm.ParabolaCast(from, velocity, Gravity);
            if (gridObject != aimedObject)
                continue;

            Vector3Int target = xy.AddZ(igm.AboveGroundLayer(xy));
            ret.Add(target);
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
}
