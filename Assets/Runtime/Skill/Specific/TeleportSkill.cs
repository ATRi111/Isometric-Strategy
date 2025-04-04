using EditorExtend.GridEditor;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "传送", menuName = "技能/特殊/传送")]
public class TeleportSkill : RangedSkill
{
    public override bool FilterOption(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int option)
    {
        GridObject gridObject = igm.GetObjectXY((Vector2Int)option);
        if (gridObject != null && !gridObject.IsGround)
            return false;
        return base.FilterOption(agent, igm, position, option);
    }

    protected override void MockOtherEffectOnAgent(IsometricGridManager igm, PawnEntity agent, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.MockOtherEffectOnAgent(igm, agent, position, target, ret);
        ret.effects.Add(new TeleportEffect(agent, position, target));
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.AppendLine("使自身移动到目标位置");
    }
}
