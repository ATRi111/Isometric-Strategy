using Character;
using UnityEngine;

public class PawnBrain : CharacterComponentBase
{
    public PawnEntity Pawn => entity as PawnEntity;

    #region ����
    public virtual float Evaluate(EffectUnit effectUnit)
    {
        return 0f;
    }
    #endregion


    #region ����
    [SerializeField]
    private SerializedHashSet<Skill> learnedSkills;

    public void Learn(Skill skill)
    {
        learnedSkills.Add(skill);
    }
    public bool IsLearned(Skill skill)
    {
        return learnedSkills.Contains(skill);
    }
    public bool Forget(Skill skill)
    {
        return learnedSkills.Remove(skill);
    }
    #endregion


    protected override void Awake()
    {
        base.Awake();
        learnedSkills = new();
    }
}
