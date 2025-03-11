using EditorExtend.GridEditor;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "����Χ��λ���",menuName = "��ֵ���۷�ʽ/����Χ��λ���")]
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
        sb.Append("��ֵ=k��ԭʼ��ֵ");
        sb.AppendLine();
        sb.Append("k��ʼΪ0��ȡֵ��ΧΪ[0,1]");
        sb.AppendLine();
        sb.Append("�뾶");
        sb.Append(radius);
        sb.Append("�ڵ�ÿ����������ʹk���");
        sb.Append(amplitude_enemy);
        sb.AppendLine();
        sb.Append("ÿ���ѷ�������ʹk����");
        sb.Append(-amplitude_ally);
        sb.AppendLine();
    }
}
