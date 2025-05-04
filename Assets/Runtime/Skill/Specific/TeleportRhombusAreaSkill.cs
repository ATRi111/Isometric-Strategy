using EditorExtend.GridEditor;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "����λ�Ƶ����η�Χ����", menuName = "����/����/����λ�Ƶ����η�Χ����")]
public class TeleportRhombusAreaSkill : RhombusAreaSkill
{
    public float powerAmplifier;
    public float speedMultiplier = 5f;
    public float jumpHeight = 5f;

    public override bool FilterOption(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int option)
    {
        GridObject gridObject = igm.GetObjectXY((Vector2Int)option);
        if (gridObject != null && !gridObject.IsGround)
            return false;
        return base.FilterOption(agent, igm, position, option);
    }

    protected override float MockDamageAmplifier(IsometricGridManager igm, PawnEntity agent, Entity victim, Vector3Int position, Vector3Int target)
    {
        return Mathf.Max(0, powerAmplifier * (position.z - target.z));
    }

    protected override void MockOtherEffectOnAgent(IsometricGridManager igm, PawnEntity agent, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        Vector3 mid = 0.5f * (Vector3)(position + target);
        mid = mid.ResetZ(Mathf.Max(position.z, target.z) + jumpHeight);
        List<Vector3> route = new()
        {
            position,
            mid,
            target,
        };
        MoveEffect move = new(agent, position, target, route, speedMultiplier);
        foreach (Effect effect in ret.effects)
        {
            effect.Join(move);
        }
        ret.effects.Add(move);
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.Append("�����λ��ÿ��Ŀ��λ�ø�1��,�˺����");
        sb.Append(powerAmplifier.ToString("P0"));
        sb.AppendLine();
    }
}
