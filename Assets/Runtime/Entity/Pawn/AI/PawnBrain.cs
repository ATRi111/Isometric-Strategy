using AStar;
using Character;
using EditorExtend.GridEditor;
using MyTool;
using Services;
using System.Collections.Generic;
using UnityEngine;

public class PawnBrain : CharacterComponentBase
{
    public PawnEntity Pawn => entity as PawnEntity;

    #region ����

    public bool humanControl;

    public List<Plan> plans;
    private readonly List<Vector3Int> options = new();
#if UNITY_EDITOR
    [SerializeField]
    private bool prepared;
#endif
    public virtual void DoAction()
    {
        if (humanControl)
        {
            Pawn.EventSystem.Invoke(EEvent.OnHumanControl, this);
        }
        else
        {
            MakePlan();
#if UNITY_EDITOR
            if(Pawn.GameManager.debug)
            {
                prepared = true;
                DebugPlanUIGenerator generator = AIManager.GetComponent<DebugPlanUIGenerator>();
                generator.brain = this;
                generator.Paint();
            }
            else
#endif
            plans[0].action.Play(Pawn.AnimationManager);
        }
    }

    public virtual void MakePlan()
    {
        plans.Clear();
        positionValueCache.Clear();
        foreach (Skill skill in learnedSkills)
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
        float primitiveValue = 0;
        List<Effect> effects = effectUnit.effects;
        for (int i = 0; i < effects.Count; i++)
        {
            primitiveValue += (float)effects[i].probability / Effect.MaxProbability * effects[i].PrimitiveValueFor(Pawn);
        }
        return primitiveValue / effectUnit.ActionTime;
    }

    private readonly Dictionary<Vector3Int, float> positionValueCache = new();

    public virtual float EvaluatePosition(Vector3Int position)
    {
        if(!positionValueCache.ContainsKey(position))
        {
            float value = 0;
            foreach (Entity entity in Pawn.Igm.EntityDict.Values)
            {
                if (Pawn.CheckFaction(entity) == -1)
                {
                    value -= IsometricGridUtility.ProjectManhattanDistance(
                        (Vector2Int)position, 
                        (Vector2Int)entity.GridObject.CellPosition);
                }
            }
            positionValueCache.Add(position, value);
        }
        return positionValueCache[position];
    }
    #endregion

    #region Ѱ·
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

    #region ����
    public SerializedHashSet<Skill> learnedSkills;

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

    protected void Update()
    {
#if UNITY_EDITOR
        if (prepared)
        {
            if (Input.GetKeyUp(KeyCode.O))
            {
                plans[0].action.Play(Pawn.AnimationManager);
                prepared = false;
            }
        }
#endif
    }
}