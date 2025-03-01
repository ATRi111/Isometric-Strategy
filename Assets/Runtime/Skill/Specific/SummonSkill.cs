using EditorExtend.GridEditor;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "召唤", menuName = "技能/特殊/召唤", order = -1)]
public class SummonSkill : RangedSkill
{
    public GameObject prefab;

    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        List<Vector2Int> primitive = IsometricGridUtility.WithinProjectManhattanDistance(castingDistance);
        for (int i = 0; i < primitive.Count; i++)
        {
            Vector2Int temp = (Vector2Int)position + primitive[i];
            if (!igm.Contains(temp))            //判断是否在地图内
                continue;
            Vector3Int target = igm.AboveGroundPosition(temp);
            if (!LayerCheck(position, target))  //判断释放位置高度差是否过大
                continue;
            GridObject gridObject = igm.GetObjectXY(temp);
            if (!gridObject.TryGetComponent(out GridSurface _))
                continue;                       //判断释放位置是否为空地
            ret.Add(target);
        }
    }

    public override void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.Mock(agent, igm, position, target, ret);
        SummonEffect effect = new(agent, prefab, target);
        ret.effects.Add(effect);
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.Append("在指定位置召唤一个");
        sb.Append(prefab.name);
        sb.AppendLine();
    }
}
