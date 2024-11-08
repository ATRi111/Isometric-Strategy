using Character;
using EditorExtend.GridEditor;
using Services;
using Services.Event;

public class Entity : CharacterEntity
{
    public IsometricGridManager Igm { get;protected set; }
    public GameManager GameManager { get; protected set; }
    public IEventSystem EventSystem { get; protected set; }
    [AutoComponent]
    public GridObject GridObject { get; protected set; }
    [AutoComponent]
    public GridMoveController MoveController { get; protected set; }
    [AutoComponent]
    public HealthComponent HealthComponent { get; protected set; }
    [AutoComponent]
    public BuffManager BuffManager { get; protected set; }

    public virtual void RefreshProperty()
    {
        HealthComponent.RefreshProperty();
    }

    protected virtual void OnTick(int time)
    {
        RefreshProperty();
    }
    protected virtual void BeforeDoAction(PawnEntity agent, int time)
    {
        RefreshProperty();
    }
    protected virtual void BeforeBattle()
    {
        RefreshProperty();
        HealthComponent.HP = HealthComponent.maxHP.CurrentValue;
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
        Igm = IsometricGridManager.FindInstance();
    }

    protected virtual void OnEnable()
    {
        GameManager.BeforeDoAction += BeforeDoAction;
        GameManager.BeforeBattle += BeforeBattle;
        GameManager.OnTick += OnTick;
        RefreshProperty();
    }

    protected virtual void OnDisable()
    {
        GameManager.BeforeDoAction -= BeforeDoAction;
        GameManager.BeforeBattle -= BeforeBattle;
        GameManager.OnTick -= OnTick;
    }
}
