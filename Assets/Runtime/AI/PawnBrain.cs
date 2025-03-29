using Character;
using Services;
using Services.Event;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制角色决策
/// </summary>
public class PawnBrain : CharacterComponentBase
{
    public static float POfNorm = 4f;

    private AIManager AIManager;
    private IEventSystem eventSystem; 
    public PawnEntity pawn;
    public PawnSensor sensor;

    public bool humanControl;

    public List<Plan> plans;
    private readonly List<Vector3Int> options = new();

    private readonly Dictionary<Vector3Int, float> positionValueCache = new();

    public Trend personality;

#if UNITY_EDITOR
    [SerializeField]
    private bool prepared;
    private DebugPlanUIGenerator generator;
#endif

    public virtual void DoAction()
    {
        pawn.EventSystem.Invoke(EEvent.BeforeDoAction, pawn);
        if (humanControl)
        {
#if UNITY_EDITOR
            generator.Clear();
#endif
        }
        else
        {
            MakePlan();
#if UNITY_EDITOR
            if (pawn.GameManager.debug)
            {
                prepared = true;
                generator.brain = this;
                generator.skillManager = pawn.SkillManager;
                generator.Paint();
            }
            else
#endif
                ExecuteAction(plans[0].action);
        }
    }

    public void ExecuteAction(PawnAction action)
    {
        action.Play(eventSystem, pawn.AnimationManager);
    }

    public PawnAction MockSkill(Skill skill, Vector3Int target)
    {
        PawnAction action = new(pawn, skill, target);
        action.Mock(pawn, pawn.Igm);
        return action;
    }

    /// <summary>
    /// 制定计划
    /// </summary>
    public virtual void MakePlan()
    {
        plans.Clear();
        positionValueCache.Clear();
        foreach (Skill skill in pawn.SkillManager.learnedSkills)
        {
            if (skill.CanUse(pawn, pawn.Igm))
                MakePlan(skill, plans);
        }
        plans.Sort();
    }
    private void MakePlan(Skill skill, List<Plan> ret)
    {
        skill.GetOptions(pawn, pawn.Igm, pawn.GridObject.CellPosition, options);
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
        skill.GetOptions(pawn, pawn.Igm, pawn.GridObject.CellPosition, options);
        for (int i = 0; i < options.Count; i++)
        {
            actions.Add(MockSkill(skill, options[i]));
        }
    }

    public virtual float Evaluate(PawnAction action)
    {
        float sum = 0;
        List<Effect> effects = action.effectUnit.effects;
        for (int i = 0; i < effects.Count; i++)
        {
            sum += (float)effects[i].probability / Effect.MaxProbability * effects[i].ValueFor(pawn);
        }
        return sum / action.Time;
    }

    public static float HealthPercent(PawnEntity pawn)
        => pawn.DefenceComponent.HP / pawn.DefenceComponent.maxHP.CurrentValue;

    /// <summary>
    /// 计算位置分
    /// </summary>
    public float EvaluatePosition(Vector3Int position)
        => EvaluateFormation(position) + EvaluateTerrain(position);

    /// <summary>
    /// 计算地形分
    /// </summary>
    private float EvaluateTerrain(Vector3Int position)
    {
        return (AIManager.trend.terrain + personality.terrain) * pawn.pClass.terrainAbility * position.z;
    }

    /// <summary>
    /// 计算阵型分
    /// </summary>
    private float EvaluateFormation(Vector3Int position)
    {
        int DistanceTo(PawnEntity other)
            => sensor.PredictDistanceBetween((Vector2Int)position, (Vector2Int)other.GridObject.CellPosition);

        float DistanceValue(float distance, float bestDistance)
        {
            if (distance <= bestDistance)
                return 0f;
            return Mathf.Pow(distance - bestDistance, 0.8f);
        }

        float SupportDistanceValue(PawnEntity supported)
            => DistanceValue(DistanceTo(supported), pawn.pClass.bestSupprtDistance);
        float SupportedDistanceValue(PawnEntity supporter)
            => DistanceValue(DistanceTo(supporter), supporter.pClass.bestSupprtDistance);

        float OffenseDistanceValue(PawnEntity victim)
            => DistanceValue(DistanceTo(victim), pawn.pClass.bestOffenseDistance);
        float DefenseDistanceValue(PawnEntity agent)
            => DistanceValue(DistanceTo(agent), agent.pClass.bestOffenseDistance);

        float H(PawnEntity pawn)
            => 0.5f + 0.5f * HealthPercent(pawn);
        float I(PawnEntity pawn)
            => 1- 0.5f * HealthPercent(pawn);

        List<PawnEntity> allies = sensor.allies;
        List<PawnEntity> enemies = sensor.enemies;

        float OfferSupport()
        {
            PNorm norm = new(POfNorm);
            for (int i = 0; i < allies.Count; i++)
            {
                norm.Add(I(allies[i]) * SupportDistanceValue(allies[i]));
            }
            return -norm.Result * pawn.pClass.supportAbility;
        }
        float SeekSupport()
        {
            PNorm norm = new(POfNorm);
            for (int i = 0; i < allies.Count; i++)
            {
                norm.Add(allies[i].pClass.supportAbility * SupportedDistanceValue(pawn));
            }
            return -norm.Result * I(pawn);
        }
        float Offense()
        {
            PNorm norm = new(POfNorm);
            for (int i = 0; i < enemies.Count; i++)
            {
                norm.Add(OffenseDistanceValue(enemies[i]));
            }
            return -norm.Result * H(pawn) * pawn.pClass.offenseAbility;
        }
        float Defense()
        {
            PNorm norm = new(POfNorm);
            for (int i = 0; i < enemies.Count; i++)
            {
                norm.Add(H(enemies[i]) * enemies[i].pClass.offenseAbility * DefenseDistanceValue(enemies[i]));
            }
            return norm.Result;
        }

        if (!positionValueCache.ContainsKey(position))
        {
            float value = (AIManager.trend + personality).Multiply(OfferSupport(), SeekSupport(), Offense(), Defense());
            positionValueCache.Add(position, value);
        }
        return positionValueCache[position];
    }

    /// <summary>
    /// 计算单位时间的天气对友方的价值
    /// </summary>
    public float EvaluateWeatherUnitTime(EWeather weather)
    {
        float sum = 0f;
        List<PawnEntity> allies = sensor.allies;
        List<PawnEntity> enemies = sensor.enemies;
        sum += EvaluateWeatherUnitTimeMyself(weather);
        for (int i = 0; i < allies.Count ; i++)
        {
            sum += allies[i].Brain.EvaluateWeatherUnitTimeMyself(weather);
        }
        for (int i = 0;i < enemies.Count ; i++)
        {
            sum -= enemies[i].Brain.EvaluateWeatherUnitTimeMyself(weather);
        }
        return sum;
    }

    /// <summary>
    /// 计算单位时间的天气对自身的价值
    /// </summary>
    public float EvaluateWeatherUnitTimeMyself(EWeather weather)
    {
        float sum = 0f;
        WeatherData data = pawn.Igm.BattleField.weatherSettings[weather];
        foreach (Skill skill in pawn.SkillManager.learnedSkills)
        {
            if(skill is AimSkill aimSkill && aimSkill.powers.Count > 0)
            {
                EDamageType type = aimSkill.powers[0].type;
                float v = data.PowerMultiplier(type) - 1f;
                sum += Mathf.Sign(v) * Mathf.Pow(v, 4f);
            }
        }
        sum = Mathf.Max(sum, 0f);
        sum = Mathf.Pow(sum, 0.25f);
        return sum;
    }

    protected override void Awake()
    {
        base.Awake();
        AIManager = ServiceLocator.Get<AIManager>();
        eventSystem = ServiceLocator.Get<IEventSystem>();
        sensor = GetComponent<PawnSensor>();
#if UNITY_EDITOR
        generator = AIManager.GetComponent<DebugPlanUIGenerator>();
#endif
        pawn = (PawnEntity)entity;
    }

    protected void Update()
    {
#if UNITY_EDITOR
        if (prepared)
        {
            if (Input.GetKeyUp(KeyCode.O))
            {
                plans[0].action.Play(eventSystem, pawn.AnimationManager);
                prepared = false;
            }
        }
#endif
    }
}
