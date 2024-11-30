using EditorExtend.GridEditor;
using MyTool;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "跳跃", menuName = "技能/跳跃")]
public class JumpSkill : RangedSkill
{
    private readonly static List<Vector2Int> Directions = new()
    {
        Vector2Int.left,
        Vector2Int.right,
        Vector2Int.up,
        Vector2Int.down,
    };

    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        //TODO:抛物线检测
        List<Entity> victims = new();
        for (int i = 0; i < Directions.Count; i++)
        {
            List<Vector2Int> route = new();
            for (int j = 0; j <= castingDistance; j++)
            {
                route.Add((Vector2Int)position + j * Directions[i]);
            }
            if (!igm.MaxLayerDict.ContainsKey(route[^1]))
                continue;
            Vector3Int target = route[^1].AddZ(igm.AboveGroundLayer(route[^1]));
            GetVictims(agent, igm, position, target, victims);
            if (victims.Count > 0)
                continue;   //不能跳到有Entity的位置
            if (agent.MovableGridObject.JumpCheck(route))
                ret.Add(target);
        }
    }

    public override void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.Mock(agent, igm, position, target, ret);
        List<Vector3Int> route = new()  //TODO:轨迹
        {
            position,
            target
        };
        ret.effects.Add(new MoveEffect(agent, route));
    }

    public override int MockTime(PawnEntity agent, Vector3Int position, Vector3Int target, IsometricGridManager igm)
    {
        return castingDistance * actionTime;
    }
}
