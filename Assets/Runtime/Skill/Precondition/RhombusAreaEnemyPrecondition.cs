using EditorExtend.GridEditor;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "��Χ���޵���", menuName = "����ǰ������/��Χ���޵���")]
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
        sb.Append("����");
        sb.Append(detectDistance);
        sb.Append("��Χ��");
        sb.Append(requireExist ? "��" : "û��");
        sb.Append("�з���ɫ");
        sb.AppendLine();
    }
}
