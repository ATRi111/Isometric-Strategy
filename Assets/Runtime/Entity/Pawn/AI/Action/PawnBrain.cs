using AStar;
using Character;
using MyTool;
using Services;
using System.Collections.Generic;
using UnityEngine;

public class PawnBrain : CharacterComponentBase
{
    public PawnEntity Pawn => entity as PawnEntity;

    #region 决策

    public bool humanControl;

    public readonly List<Plan> plans = new();
    private readonly List<Vector3Int> options = new();

    public virtual void DoAction()
    {
        if (humanControl)
        {
            Pawn.EventSystem.Invoke(EEvent.OnHumanControl, this);
        }
        else
        {
            MakePlan();
            plans[0].action.Excute();
        }
    }

    public virtual void MakePlan()
    {
        plans.Clear();
        foreach(Skill skill in learnedSkills)
        {
            MakePlan(skill);
        }
        plans.Sort();
    }

    private void MakePlan(Skill skill)
    {
        skill.GetOptions(Pawn, Pawn.Igm, Pawn.GridObject.CellPosition, options);
        for(int i = 0; i < options.Count; i++)
        {
            PawnAction action = new(Pawn, skill, options[i]);
            action.Mock(Pawn, Pawn.Igm);
            Plan plan = new(action);
            plans.Add(plan);
        }
    }

    public virtual float Evaluate(EffectUnit effectUnit)
    {
        return 0f;
    }
    #endregion

    #region 寻路
    public AIManager AIManager { get; private set; }

    public void FindAvalable(Vector3Int from, List<Vector3Int> ret)
    {
        ret.Clear();
        MovableGridObject gridObject = Pawn.GridObject as MovableGridObject;
        PathFindingProcess process = AIManager.PathFinding.FindAvailable(gridObject.Mover, (Vector2Int)from);
        for (int i = 0; i < process.available.Count; i++)
        {
            ret.Add((process.available[i] as ANode).CellPositon);
        }
    }

    public void FindRoute(Vector3Int from, Vector3Int to, List<Vector3Int> ret)
    {
        ret.Clear();
        MovableGridObject gridObject = Pawn.GridObject as MovableGridObject;
        ret.Add(gridObject.CellPosition);
        PathFindingProcess process = AIManager.PathFinding.FindRoute(gridObject.Mover, (Vector2Int)from, (Vector2Int)to);
        for (int i = 0; i < process.output.Count; i++)
        {
            ret.Add((process.output[i] as ANode).CellPositon);
        }
    }
    #endregion

    #region 技能
    [SerializeField]
    private SerializedHashSet<Skill> learnedSkills;

    public void GetSkills(List<Skill> ret)
    {
        ret.Clear();
        ret.AddRange(learnedSkills);
    }

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
        AIManager = ServiceLocator.Get<AIManager>();
    }
}
