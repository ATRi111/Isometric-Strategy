using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "跳跃", menuName = "技能/跳跃")]
public class JumpSkill : RangedSkill
{
    public static float jumpAngle = 30f;
    
    private readonly static List<Vector2Int> Directions = new()
    {
        Vector2Int.left,
        Vector2Int.right,
        Vector2Int.up,
        Vector2Int.down,
    };

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

            Vector3 from = agent.GridObject.BottomCenter;
            Vector3 to = gridObject.TopCenter;
            gridObject = igm.ParabolaCast(from, to, jumpAngle, out Vector3 _);
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
        GridObject gridObject = igm.GetObjectXY((Vector2Int)target);
        Vector3 from = agent.GridObject.BottomCenter;
        Vector3 to = gridObject.TopCenter;
        GridPhysics.InitialVelocityOfParabola(from, to, jumpAngle, GridPhysics.settings.gravity, out Vector3 velocity, out float time);
        GridPhysics.DiscretizeParabola(from, velocity, GridPhysics.settings.gravity, time, GridPhysics.settings.parabolaPrecision, route);
        ret.effects.Add(new MoveEffect(agent, position, target, route));
    }

    public override int MockTime(PawnEntity agent, Vector3Int position, Vector3Int target, IsometricGridManager igm)
    {
        int time = 0;
        for (int i = 0; i < MoveSkill.ZoneOfControl.Length; i++)
        {
            Vector3Int temp = position + target;
            if (igm.EntityDict.ContainsKey(temp) && agent.CheckFaction(igm.EntityDict[temp]) == -1)
                time += actionTime;
        }
        return (castingDistance + 1) * actionTime;
    }
}
