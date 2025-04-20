using EditorExtend.GridEditor;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "��Ӱɱ��", menuName = "����/����/��Ӱɱ��")]
public class ShadowTeleportSkill : TeleportSkill
{
    public override bool FilterOption(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int option)
    {
        if (!base.FilterOption(agent, igm, position, option))
            return false;
        List<Vector2Int> adjoins = IsometricGridUtility.WithinProjectManhattanDistance(1);
        for (int i = 0; i < adjoins.Count; i++)
        {
            Vector3Int p = igm.AboveGroundPosition((Vector2Int)option + adjoins[i]);
            if(igm.entityDict.ContainsKey(p) 
                && igm.entityDict[p] is PawnEntity pawn
                && (Vector2Int)pawn.GridObject.CellPosition - pawn.faceDirection == (Vector2Int)option)
            {
                return true;
            }
        }
        return false;
    }

    protected override void DescribeTeleport(StringBuilder sb)
    {
        sb.AppendLine("�������͵�һ����λ�ı���");
    }
}
