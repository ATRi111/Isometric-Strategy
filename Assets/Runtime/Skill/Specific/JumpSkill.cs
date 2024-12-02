using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "跳跃", menuName = "技能/跳跃")]
public class JumpSkill : MoveSkill
{
    public int castingDistance = 2;
    public float jumpAngle = 45f;
    public float jumpOffset = 0.5f;

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
            GridObject gridObject = igm.GetObjectXY(xy);
            if(gridObject == null)
                continue;
            if (gridObject.GetComponent<Entity>() != null)  //不可落在Entity上
                continue;

            Vector3 from = agent.GridObject.BottomCenter + jumpOffset * Vector3.forward;
            Vector3 to = gridObject.TopCenter + jumpOffset * Vector3.forward;
            if (!GridPhysics.InitialVelocityOfParabola(from, to, jumpAngle, Gravity, out Vector3 velocity, out float time))
                continue;
            gridObject = igm.ParabolaCast(from, velocity,Gravity, time, out Vector3 _);
            if (gridObject != null) //不可穿过GridObject
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
