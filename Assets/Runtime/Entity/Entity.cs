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

    public virtual void Refresh()
    {
        
    }

    protected virtual void Register()
    {
        GameManager.AfterTimeChange += AfterTimeChange;
        GameManager.OnStartBattle += OnStartBattle;
    }
    protected virtual void UnRegister()
    {
        GameManager.AfterTimeChange -= AfterTimeChange;
        GameManager.OnStartBattle -= OnStartBattle;
    }
    protected virtual void AfterTimeChange(int prev, int current)
    {
        Refresh();
    }
    protected virtual void OnStartBattle()
    {
        Refresh();
        HealthComponent.HP = HealthComponent.maxHP;
    }

    public virtual void Die()
    {
        gameObject.SetActive(false);
    }

    public virtual void Revive()
    {
        gameObject.SetActive(true);
        Refresh();
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
        Register();
    }

    protected virtual void OnDisable()
    {
        UnRegister();
    }
}
