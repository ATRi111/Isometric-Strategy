using Character;
using EditorExtend.GridEditor;
using Services;
using Services.Event;
using UnityEngine;

public class Entity : EntityBase
{
    public IsometricGridManager Igm { get;protected set; }
    public GameManager GameManager { get; protected set; }
    public AnimationManager AnimationManager { get; protected set; }
    public IEventSystem EventSystem { get; protected set; }
    [AutoComponent]
    public GridObject GridObject { get; protected set; }
    [AutoComponent]
    public GridMoveController MoveController { get; protected set; }
    [AutoComponent]
    public BattleComponent BattleComponent { get; protected set; }
    [AutoComponent]
    public BuffManager BuffManager { get; protected set; }

    [SerializeField]
    private string entityName;
    public string EntityName
    {
        get
        {
            if(string.IsNullOrEmpty(entityName))
                return gameObject.name;
            return entityName;
        }
    }

    public virtual void RefreshProperty()
    {
        BattleComponent.Refresh();
    }

    protected virtual void OnTick(int time)
    {
        BuffManager.Refresh(time);
        RefreshProperty();
    }

    protected virtual void BeforeBattle()
    {
        RefreshProperty();
        BattleComponent.HP = BattleComponent.maxHP.IntValue;
    }

    public virtual void Die()
    {
        gameObject.SetActive(false);
    }

    public virtual void Revive()
    {
        gameObject.SetActive(true);
    }

    protected override void Awake()
    {
        base.Awake();
        GameManager = ServiceLocator.Get<GameManager>();
        EventSystem = ServiceLocator.Get<IEventSystem>();
        AnimationManager = ServiceLocator.Get<AnimationManager>();
        Igm = IsometricGridManager.FindInstance();
    }

    protected virtual void OnEnable()
    {
        EventSystem.AddListener(EEvent.BeforeBattle, BeforeBattle);
        EventSystem.AddListener<int>(EEvent.OnTick, OnTick);
    }

    protected virtual void OnDisable()
    {
        EventSystem.RemoveListener(EEvent.BeforeBattle, BeforeBattle);
        EventSystem.RemoveListener<int>(EEvent.OnTick, OnTick);
    }
}
