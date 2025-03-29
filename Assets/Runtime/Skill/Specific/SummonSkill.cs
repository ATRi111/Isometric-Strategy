using EditorExtend.GridEditor;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "�ٻ�", menuName = "����/����/�ٻ�", order = -1)]
public class SummonSkill : RangedSkill
{
    public GameObject prefab;

    public override bool FilterOption(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int option)
    {
        GridObject gridObject = igm.GetObjectXY((Vector2Int)option);
        if (gridObject != null && !gridObject.IsGround)
            return false;
        return base.FilterOption(agent, igm, position, option);
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
        sb.Append("��ָ��λ���ٻ�һ��");
        if(prefab != null)
            sb.Append(prefab.name);
        sb.AppendLine();
    }
}
