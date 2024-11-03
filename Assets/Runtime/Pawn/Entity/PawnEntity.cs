using Character;
using Services;
using Services.Event;
using UnityEngine;

public class PawnEntity : CharacterEntity
{
    public GameManager GameManager { get; private set; }
    public IEventSystem EventSystem { get; private set; }
    [AutoComponent]
    public MovableGridObject GridObject { get; private set; }
    [AutoComponent]
    public PawnBrain Brain { get; private set; }
    [AutoComponent]
    public GridMoveController MoveController { get; private set; }

    [SerializeField]
    private PawnProperty defaultProperty;
    [SerializeField]
    private PawnProperty property;
    public PawnProperty Property => property;

    [SerializeField]
    private PawnSetting setting;
    public PawnSetting Setting => setting;

    [SerializeField]
    private PawnState state;
    public PawnState State => state;

    protected override void Awake()
    {
        base.Awake();
        state.MaxHP = () => Property.maxHP;
        GameManager = ServiceLocator.Get<GameManager>();
        EventSystem = ServiceLocator.Get<IEventSystem>();
    }

    private void OnEnable()
    {
        Initialize();
        Register();
    }

    private void OnDisable()
    {
        UnRegister();
    }

    public void Initialize()
    {
        property = defaultProperty.Clone() as PawnProperty;
        state.HP = Property.maxHP;
    }


    private void Register()
    {
        GameManager.Register(this);
        GameManager.AfterTimeChange += AfterTimeChange;
        GameManager.OnStartBattle += OnStartBattle;
    }
    private void UnRegister()
    {
        GameManager.Unregister(this);
        GameManager.AfterTimeChange -= AfterTimeChange;
        GameManager.OnStartBattle -= OnStartBattle;
    }
    private void AfterTimeChange(int prev, int current)
    {
        Refresh();
    }
    private void OnStartBattle()
    {
        Refresh();
        state.waitTime = property.actionTime;   //»Î≥°AT
    }
    public void Refresh()
    {
        property = defaultProperty.Clone() as PawnProperty;
    }

    public void DoAction()
    {
        if (setting.humanControl)
            EventSystem.Invoke(EEvent.OnHumanControl, this);
        else
            Brain.DoAction();
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }

    public void Revive()
    {
        gameObject.SetActive(true);
        Refresh();
    }
}