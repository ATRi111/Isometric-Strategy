using UnityEngine;

/// <summary>
/// һ������
/// </summary>
public class PawnAction
{
    public Skill skill;
    public Vector2Int target;

    public PawnAction(Skill skill, Vector2Int target)
    {
        this.skill = skill;
        this.target = target;
    }

    /// <summary>
    /// ģ�⶯��������Ӱ��
    /// </summary>
    public void Mock(PawnEntity agent, IsometricGridManager igm, EffectUnit ret)
    {
        skill.Mock(agent, target, igm, ret);
    }
}
