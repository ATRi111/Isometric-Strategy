using Character;
using MyTool;
using Services;
using Services.Event;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : CharacterComponentBase
{
    private IEventSystem eventSystem;
    private PawnEntity pawn;
    public SerializedHashSet<Skill> learnedSkills;

    public readonly HashSet<Vector3Int> offenseArea = new();

    public void Learn(Skill skill)
    {
        learnedSkills.Add(skill);
    }
    /// <summary>
    /// 按类型查找技能
    /// </summary>
    public T FindSkill<T>() where T : Skill
    {
        foreach (Skill skill in learnedSkills)
        {
            if (skill is T)
                return skill as T;
        }
        return null;
    }
    /// <summary>
    /// 按名称和类型查找技能（优先看是否有展示名称包含name的技能，再看是否有资产名称包含name的技能）
    /// </summary>
    public T FindSkill<T>(string name) where T : Skill
    {
        foreach (Skill skill in learnedSkills)
        {
            if (skill is T && skill.displayName == name)
                return skill as T;
        }
        foreach (Skill skill in learnedSkills)
        {
            if (skill is T && skill.name.Contains(name))
                return skill as T;
        }
        return null;
    }
    public bool Forget(Skill skill)
    {
        return learnedSkills.Remove(skill);
    }

    public void PredictOffenseArea()
    {
        offenseArea.Clear();
        List<Vector3Int> temp = new();
        foreach (Skill skill in learnedSkills)
        {
            if (skill is RangedSkill rangedSkill)
            {
                if (rangedSkill.Offensive)
                    rangedSkill.GetOptions(pawn, pawn.Igm, pawn.GridObject.CellPosition, temp);
            }
            for (int i = 0; i < temp.Count; i++)
            {
                offenseArea.Add(temp[i]);
            }
        }
    }

    private void BeforeDoAction(PawnEntity agent)
    {
        if (agent.Brain.humanControl)
            PredictOffenseArea();   //自身为敌人，且轮到玩家行动时，更新威胁范围
        else
            offenseArea.Clear();
    }

    /// <summary>
    /// 使技能中涉及的参数出现在字典中
    /// </summary>
    private void InitParameter()
    {
        HashSet<string> parameterNames = new();
        for (int i = 0; i < learnedSkills.Count; i++)
        {
            learnedSkills[i].GetRelatedParameters(parameterNames);
        }
        foreach (string parameterName in parameterNames)
        {
            pawn.parameterDict[parameterName] += 0;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        eventSystem = ServiceLocator.Get<IEventSystem>();
        pawn = (PawnEntity)entity;
    }

    private void OnEnable()
    {
        if (pawn.faction == EFaction.Enemy)
            eventSystem.AddListener<PawnEntity>(EEvent.BeforeDoAction, BeforeDoAction);
        InitParameter();
    }

    private void OnDisable()
    {
        if (pawn.faction == EFaction.Enemy)
            eventSystem.RemoveListener<PawnEntity>(EEvent.BeforeDoAction, BeforeDoAction);
    }

    private void Start()
    {
        if (pawn.faction == EFaction.Enemy)
            PredictOffenseArea();
    }
}
