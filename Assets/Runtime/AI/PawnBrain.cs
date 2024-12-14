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
    private DebugPlanUIGenerator generator;
#endif
    public virtual void DoAction()
    {
        if (humanControl)
        {
#if UNITY_EDITOR
            generator.Clear();
#endif
            Pawn.EventSystem.Invoke(EEvent.OnHumanControl, Pawn);
        }
        else
        {
            MakePlan();
#if UNITY_EDITOR
            if (Pawn.GameManager.debug)
            {
                prepared = true;
                generator.brain = this;
                generator.Paint();
            }
            else
#endif
                ExecuteAction(plans[0].action);
        }
    }

    public void ExecuteAction(PawnAction action)
    {
        action.Play(Pawn.AnimationManager);
    }

    public PawnAction MockSkill(Skill skill, Vector3Int target)
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
            if (skill.CanUse(Pawn, Pawn.Igm))
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
            int flag = Pawn.FactionCheck(pawn);
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
    public virtual float Evaluate(PawnAction action)
    {
        float sum = 0;
        List<Effect> effects = action.effectUnit.effects;
        for (int i = 0; i < effects.Count; i++)
        {
            sum += (float)effects[i].probability / Effect.MaxProbability * effects[i].PrimitiveValueFor(Pawn);
        }
        return sum / action.Time;
    }

    public float HealthPercent(PawnEntity pawn)
        => pawn.BattleComponent.HP / pawn.BattleComponent.maxHP.CurrentValue;

    public virtual float EvaluatePosition(Vector3Int position)
    {
        Dictionary<Vector2Int, ANode> nodeDict = new();
        Ranging(position, nodeDict);
        int DistanceTo(PawnEntity other)
        {
            Vector2Int xy = (Vector2Int)other.GridObject.CellPosition;
            if (nodeDict.ContainsKey(xy))
                return Mathf.RoundToInt(nodeDict[xy].GCost);
            Debug.LogWarning($"{other.gameObject.name}不可达");
            return IsometricGridUtility.ProjectManhattanDistance((Vector2Int)position, xy);
        }

        int SupportDistance(PawnEntity supported)
            => -Mathf.Abs(DistanceTo(supported) - Pawn.pClass.bestSupprtDistance);
        int SupportedDistance(PawnEntity supporter)
            => -Mathf.Abs(DistanceTo(supporter) - supporter.pClass.bestSupprtDistance);

        int OffenseDistance(PawnEntity victim)
            => -Mathf.Abs(DistanceTo(victim) - Pawn.pClass.bestOffenseDistance);
        int DefenseDistance(PawnEntity agent)
            => -Mathf.Abs(DistanceTo(agent) - agent.pClass.bestOffenseDistance);

        float H(PawnEntity pawn)
            => 0.5f + 0.5f * HealthPercent(pawn);
        float I(PawnEntity pawn)
            => 1- 0.5f * HealthPercent(pawn);

        float OfferSupport()
        {
            float sum = 0;
            for (int i = 0; i < allies.Count; i++)
            {
                sum += I(allies[i]) * SupportDistance(allies[i]);
            }
            sum *= Pawn.pClass.supportAbility;
            return sum;
        }
        float SeekSupport()
        {
            float sum = 0;
            for (int i = 0; i < allies.Count; i++)
            {
                sum += allies[i].pClass.supportAbility * SupportedDistance(Pawn);
            }
            sum *= I(Pawn);
            return sum;
        }
        float Offense()
        {
            float sum = 0;
            for (int i = 0; i < enemies.Count; i++)
            {
                sum += OffenseDistance(enemies[i]);
            }
            sum *= H(Pawn) * Pawn.pClass.offenseAbility;
            return sum;
        }
        float Defense()
        {
            float sum = 0;
            for (int i = 0; i < enemies.Count; i++)
            {
                sum -= H(enemies[i]) * enemies[i].pClass.offenseAbility * DefenseDistance(enemies[i]);
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

    public void FindAvailable(Vector3Int from, List<Vector3Int> ret)
    {
        ret.Clear();
        PathFindingProcess process = AIManager.PathFinding.FindAvailable(Pawn.MovableGridObject.Mover_Default, (Vector2Int)from);
        for (int i = 0; i < process.available.Count; i++)
        {
            ret.Add((process.available[i] as ANode).CellPosition);
        }
    }

    public void FindRoute(Vector3Int from, Vector3Int to, List<Vector3Int> ret)
    {
        ret.Clear();
        ret.Add(Pawn.MovableGridObject.CellPosition);
        PathFindingProcess process = AIManager.PathFinding.FindRoute(Pawn.MovableGridObject.Mover_Default, (Vector2Int)from, (Vector2Int)to);
        for (int i = 0; i < process.output.Count; i++)
        {
            ret.Add((process.output[i] as ANode).CellPosition);
        }
    }

    public void Ranging(Vector3Int from, Dictionary<Vector2Int, ANode> ret)
    {
        ret.Clear();
        PathFindingProcess process = AIManager.PathFinding.Ranging(Pawn.MovableGridObject.Mover_Ranging, (Vector2Int)from);
        for (int i = 0; i < process.available.Count; i++)
        {
            ANode node = process.available[i] as ANode;
            ret.Add(node.Position, node);
        }
    }
    #endregion

    #region 技能
    public SerializedHashSet<Skill> learnedSkills;

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
            if(skill is T && skill.displayName == name)
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
    #endregion

    protected override void Awake()
    {
        base.Awake();
        AIManager = ServiceLocator.Get<AIManager>();
#if UNITY_EDITOR
        generator = AIManager.GetComponent<DebugPlanUIGenerator>();
#endif
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
