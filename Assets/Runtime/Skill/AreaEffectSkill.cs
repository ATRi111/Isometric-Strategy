using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AOE技能", menuName = "技能/AOE技能", order = -1)]
public class AreaEffectSkill : RangedSkill
{
    public int effectRange = 1;

    public override void GetVictims(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Entity> ret)
    {
        ret.Clear();
        List<Vector2Int> primitive = IsometricGridUtility.WithinProjectManhattanDistance(effectRange);
        for (int i = 0; i < primitive.Count; i++)
        {
            Vector2Int xy = (Vector2Int)target + primitive[i];
            Vector3Int temp = xy.AddZ(igm.AboveGroundLayer(xy));
            if (igm.EntityDict.ContainsKey(temp) && FilterVictim(igm.EntityDict[temp]))
                ret.Add(igm.EntityDict[temp]);
        }
    }
}
