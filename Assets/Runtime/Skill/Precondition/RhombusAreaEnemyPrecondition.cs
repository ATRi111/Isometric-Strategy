using EditorExtend.GridEditor;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "周围有无敌人", menuName = "技能前置条件/周围有无敌人")]
public class RhombusAreaEnemyPrecondition : SkillPrecondition
{
    public int detectDistance;
    public bool requireExist;

    public override bool Verify(IsometricGridManager igm, PawnEntity agent)
    {
        List<PawnEntity> enemeis = agent.Sensor.enemies;
        for (int i = 0; i < enemeis.Count; i++)
        {
            int distance = IsometricGridUtility.ProjectManhattanDistance(
                (Vector2Int)enemeis[i].GridObject.CellPosition, 
                (Vector2Int)agent.GridObject.CellPosition);
            if (distance <= detectDistance)
                return requireExist;
        }
        return !requireExist;
    }

    public override void Describe(StringBuilder sb)
    {
        sb.Append("自身");
        sb.Append(detectDistance);
        sb.Append("格范围内");
        sb.Append(requireExist ? "有" : "没有");
        sb.Append("敌方角色");
        sb.AppendLine();
    }
}
