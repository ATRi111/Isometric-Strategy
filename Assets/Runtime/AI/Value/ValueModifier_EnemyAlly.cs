using EditorExtend.GridEditor;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "与周围单位相关",menuName = "价值评价方式/与周围单位相关")]
public class ValueModifier_EnemyAlly : ValueModifier
{
    public int radius;
    public float amplitude_enemy;
    public float amplitude_ally;

    public override float CalculateValue(float primitiveValue, PawnEntity victim)
    {
        float DistanceValue(PawnEntity pawn)
        {
            int distance = IsometricGridUtility.ProjectManhattanDistance((Vector2Int)victim.GridObject.CellPosition, (Vector2Int)pawn.GridObject.CellPosition);
            return 1f / distance;
        }

        List<PawnEntity> enemeis = victim.Sensor.enemies;
        List<PawnEntity> allies = victim.Sensor.allies;
        float k = 0;
        for (int i = 0; i < enemeis.Count; i++)
        {
            k += amplitude_enemy * DistanceValue(enemeis[i]);
        }
        for (int i = 0; i < allies.Count; i++)
        {
            k += amplitude_ally * DistanceValue(allies[i]);
        }
        return Mathf.Clamp01(k) * primitiveValue;
    }

    protected override void Describe(StringBuilder sb)
    {
        sb.Append("价值=k×原始价值");
        sb.AppendLine();
        sb.Append("k初始为0，取值范围为[0,1]");
        sb.AppendLine();
        sb.Append("半径");
        sb.Append(radius);
        sb.Append("内的每个敌人至多使k提高");
        sb.Append(amplitude_enemy);
        sb.AppendLine();
        sb.Append("每个友方则至多使k降低");
        sb.Append(-amplitude_ally);
        sb.AppendLine();
    }
}
