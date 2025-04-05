using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "相对高度差影响伤害的技能", menuName = "技能/特殊/相对高度差影响伤害的技能")]
public class RelativeHeightSkill : RangedSkill
{
    public bool higher;
    public float maxAmplifier;
    public float damageAmplifier;

    protected override float MockDamageAmplifier(IsometricGridManager igm, PawnEntity agent, Entity victim, Vector3Int position, Vector3Int target)
    {
        int h = agent.GridObject.CellPosition.z - victim.GridObject.CellPosition.z;
        if (!higher)
            h = -h;
        return Mathf.Clamp(h * damageAmplifier, 0, maxAmplifier);
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.Append("自身的位置每比目标");
        if (higher)
            sb.Append("高");
        else
            sb.Append("低");
        sb.Append("一格,");
        sb.AppendLine();
        sb.Append("伤害提高");
        sb.Append(damageAmplifier.ToString("P0"));
        sb.Append("(最高");
        sb.Append(maxAmplifier.ToString("P0"));
        sb.Append(")");
    }
}
