using UnityEngine;

/// <summary>
/// һ������
/// </summary>
[System.Serializable]
public class PawnAction
{
    public PawnEntity agent;
    public Skill skill;
    public Vector3Int target;
    public EffectUnit effectUnit;

    public PawnAction(PawnEntity agent, Skill skill, Vector3Int target)
    {
        this.skill = skill;
        this.target = target;
        this.agent = agent;
        effectUnit = new(agent);
    }

    /// <summary>
    /// ģ�⶯��������Ӱ��
    /// </summary>
    public void Mock(PawnEntity agent, IsometricGridManager igm)
    {
        skill.Mock(agent, igm, agent.GridObject.CellPosition, target, effectUnit);
    }

    /// <summary>
    /// ִ�в����Ŷ���
    /// </summary>
    public void Play(AnimationManager animationManager)
    {
        effectUnit.Play(animationManager);
        animationManager.StartAnimationCheck();
    }
}
