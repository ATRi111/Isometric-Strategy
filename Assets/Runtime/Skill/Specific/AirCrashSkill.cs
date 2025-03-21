using EditorExtend.GridEditor;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "�ѿ�׹��", menuName = "����/����/�ѿ�׹��")]
public class AirCrashSkill : RhombusAreaSkill
{
    public float powerAmplifier;
    public float speedMultiplier = 5f;

    public override void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        base.GetOptions(agent, igm, position, ret);
        List<Vector3Int> temp = new();
        for (int i = 0; i < ret.Count; i++)
        {
            GridObject gridObject = igm.GetObjectXY((Vector2Int)ret[i]);
            if (gridObject.IsGround)
                temp.Add(ret[i]);
        }
        ret.Clear();
        ret.AddRange(temp);
    }

    protected override float MockDamageAmplifier(IsometricGridManager igm, PawnEntity agent, Entity victim, Vector3Int position, Vector3Int target)
    {
        return Mathf.Max(0, powerAmplifier * (position - target).z);
    }

    public override void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.Mock(agent, igm, position, target, ret);
        Vector3 mid = 0.5f * (Vector3)(position + target);
        mid.ResetZ(Mathf.Max(position.z, target.z));
        List<Vector3> route = new()
        {
            position,
            mid,
            target,
        };
        MoveEffect move = new(agent, position, target, route, 5);
        foreach(Effect effect in ret.effects)
        {
            effect.Join(move);
        }
        ret.effects.Add(new MoveEffect(agent, position, target, route, speedMultiplier));
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.AppendLine("ʹ�����ƶ���Ŀ��λ��");
        sb.Append("�����λ��ÿ��Ŀ��λ�ø�1��,�˺����");
        sb.Append(powerAmplifier.ToString("P0"));
        sb.AppendLine();
    }
}
