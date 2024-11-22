using AStar;
using Character;
using EditorExtend.GridEditor;
using MyTool;
using Services;
using Services.Event;
using System.Collections.Generic;
using UnityEngine;

public class PawnBrain : CharacterComponentBase
{
    public PawnEntity Pawn => entity as PawnEntity;

    #region 决策

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
            if (Pawn.GameManager.debug)
            {
                prepared = true;
                DebugPlanUIGenerator generator = AIManager.GetComponent<DebugPlanUIGenerator>();
                generator.brain = this;
                generator.Paint();
            }
            else
#endif
                ExcuteAction(plans[0].action);
        }
    }

    public void ExcuteAction(PawnAction action)
    {
        action.Play(Pawn.AnimationManager);
    }

    public PawnAction MockSkill(Skill skill,Vector3Int target)
    {
        PawnAction action = new(Pawn, skill, target);
        action.Mock(Pawn, Pawn.Igm);
        return action;
    }

    private readonly Dictionary<Vector3Int, float> positionValueCache = new();
    private readonly List<PawnEntity> allies = new();
    private readonly List<PawnEntity> enemies = new();

    /// <summary>
    /// 制定计划
    /// </summary>
    public virtual void MakePlan()
    {
        plans.Clear();
        RecognizeEnemyAndAlly();
        positionValueCache.Clear();
        foreach (Skill skill in learnedSkills)
        {
            MakePlan(skill, plans);
        }
        plans.Sort();
    }
    private void MakePlan(Skill skill, List<Plan> ret)
    {
        skill.GetOptions(Pawn, Pawn.Igm, Pawn.GridObject.CellPosition, options);
        for(int i = 0; i < options.Count; i++)
        {
            PawnAction action = MockSkill(skill, options[i]);
            ret.Add(new Plan(action));
        }
    }

    /// <summary>
    /// 仅获取可选行动而不评估
    /// </summary>
    public void MakeAction(Skill skill, List<PawnAction> actions)
    {
        actions.Clear();
        skill.GetOptions(Pawn, Pawn.Igm, Pawn.GridObject.CellPosition, options);
        for (int i = 0; i < options.Count; i++)
        {
            actions.Add(MockSkill(skill, options[i]));
        }
    }

    public void RecognizeEnemyAndAlly()
    {
        allies.Clear();
        enemies.Clear();
        foreach(PawnEntity pawn in Pawn.GameManager.pawns)
        {
            if(pawn == Pawn)
                continue;
            int flag = Pawn.CheckFaction(pawn);
            switch(flag)
            {
                case 1:
                    allies.Add(pawn);
                    break;
                case -1:
                    enemies.Add(pawn);
                    break;
                case 0:
                    break;
            }
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

    public float HealthPercent(PawnEntity pawn)
        => pawn.BattleComponent.HP / pawn.BattleComponent.maxHP.CurrentValue;

    public virtual float EvaluatePosition(Vector3Int position)
    {
        int Distance(PawnEntity x,PawnEntity y)
        {
            Vector3Int px, py; 
            if(x == Pawn)
            {
                px = position;
                py = y.GridObject.CellPosition;
            }
            else if (y == Pawn)
            {
                px = x.GridObject.CellPosition;
                py = position;
            }
            else
            {
                px = x.GridObject.CellPosition;
                py = y.GridObject.CellPosition;
            }
            return IsometricGridUtility.ProjectManhattanDistance((Vector2Int)px, (Vector2Int)py);
        }
        int SupportDistance(PawnEntity supporter, PawnEntity supportee)
            => -Mathf.Abs(Distance(supporter, supportee) - supporter.pClass.bestSupprtDistance);
        int OffenseDistance(PawnEntity agent,PawnEntity victim)
            => -Mathf.Abs(Distance(agent, victim) - agent.pClass.bestOffenseDistance);
        float H(PawnEntity pawn)
            => 0.5f + 0.5f * HealthPercent(pawn);
        float I(PawnEntity pawn)
            => 1- 0.5f * HealthPercent(pawn);

        float OfferSupport()
        {
            float sum = 0;
            for (int i = 0; i < allies.Count; i++)
            {
                sum += I(allies[i]) * SupportDistance(Pawn, allies[i]);
            }
            sum *= Pawn.pClass.supportAbility;
            return sum;
        }
        float SeekSupport()
        {
            float sum = 0;
            for (int i = 0; i < allies.Count; i++)
            {
                sum += allies[i].pClass.supportAbility * SupportDistance(allies[i], Pawn);
            }
            sum *= I(Pawn);
            return sum;
        }
        float Offense()
        {
            float sum = 0;
            for (int i = 0; i < enemies.Count; i++)
            {
                sum += OffenseDistance(Pawn, enemies[i]);
            }
            sum *= H(Pawn) * Pawn.pClass.offenseAbility;
            return sum;
        }
        float Defense()
        {
            float sum = 0;
            for (int i = 0; i < enemies.Count; i++)
            {
                sum -= H(enemies[i]) * enemies[i].pClass.offenseAbility * OffenseDistance(enemies[i], Pawn);
            }
            return sum;
        }

        if (!positionValueCache.ContainsKey(position))
        {
            float value = AIManager.trend.Multiply(OfferSupport(), SeekSupport(), Offense(), Defense());
            positionValueCache.Add(position, value);
        }
        return positionValueCache[position];
    }
    #endregion

    #region 寻路
    public AIManager AIManager { get; private set; }

    public void FindAvalable(Vector3Int from, List<Vector3Int> ret)
    {
        ret.Clear();
        PathFindingProcess process = AIManager.PathFinding.FindAvailable(Pawn.MovableGridObject.Mover, (Vector2Int)from);
        for (int i = 0; i < process.available.Count; i++)
        {
            ret.Add((process.available[i] as ANode).CellPositon);
        }
    }

    public void FindRoute(Vector3Int from, Vector3Int to, List<Vector3Int> ret)
    {
        ret.Clear();
        ret.Add(Pawn.MovableGridObject.CellPosition);
        PathFindingProcess process = AIManager.PathFinding.FindRoute(Pawn.MovableGridObject.Mover, (Vector2Int)from, (Vector2Int)to);
        for (int i = 0; i < process.output.Count; i++)
        {
            ret.Add((process.output[i] as ANode).CellPositon);
        }
    }
    #endregion

    #region 技能
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
