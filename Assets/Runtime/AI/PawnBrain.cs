using Character;
using Services;
using Services.Event;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ƽ�ɫ����
/// </summary>
public class PawnBrain : CharacterComponentBase
{
    private AIManager AIManager;
    public PawnEntity Pawn => entity as PawnEntity;
    public PawnSensor sensor;

    public bool humanControl;

    public List<Plan> plans;
    private readonly List<Vector3Int> options = new();

    private readonly Dictionary<Vector3Int, float> positionValueCache = new();

#if UNITY_EDITOR
    [SerializeField]
    private bool prepared;
    private DebugPlanUIGenerator generator;
#endif

    public virtual void DoAction()
    {
        Pawn.EventSystem.Invoke(EEvent.BeforeDoAction, Pawn);
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
            if (Pawn.GameManager.debug)
            {
                prepared = true;
                generator.brain = this;
                generator.skillManager = Pawn.SkillManager;
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

    /// <summary>
    /// �ƶ��ƻ�
    /// </summary>
    public virtual void MakePlan()
    {
        plans.Clear();
        positionValueCache.Clear();
        foreach (Skill skill in Pawn.SkillManager.learnedSkills)
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
    /// ����ȡ��ѡ�ж���������
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

    public virtual float Evaluate(PawnAction action)
    {
        float sum = 0;
        List<Effect> effects = action.effectUnit.effects;
        for (int i = 0; i < effects.Count; i++)
        {
            sum += (float)effects[i].probability / Effect.MaxProbability * effects[i].ValueFor(Pawn);
        }
        return sum / action.Time;
    }

    public static float HealthPercent(PawnEntity pawn)
        => pawn.DefenceComponent.HP / pawn.DefenceComponent.maxHP.CurrentValue;

    public float EvaluatePosition(Vector3Int position)
        => EvaluateFormation(position) + EvaluateTerrain(position);

    /// <summary>
    /// ������η�
    /// </summary>
    protected virtual float EvaluateTerrain(Vector3Int position)
    {
        return AIManager.trend.terrain * position.z;
    }

    /// <summary>
    /// �������ͷ�
    /// </summary>
    protected virtual float EvaluateFormation(Vector3Int position)
    {
        int DistanceTo(PawnEntity other)
            => sensor.PredictDistanceBetween((Vector2Int)position, (Vector2Int)other.GridObject.CellPosition);

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

        List<PawnEntity> allies = sensor.allies;
        List<PawnEntity> enemies = sensor.enemies;

        float OfferSupport()
        {
            float sum = 0;
            for (int i = 0; i < sensor.allies.Count; i++)
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

    protected override void Awake()
    {
        base.Awake();
        AIManager = ServiceLocator.Get<AIManager>();
        sensor = GetComponent<PawnSensor>();
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
