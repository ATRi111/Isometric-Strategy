using Character;
using Services;
using UnityEngine;

public class PawnEntity : CharacterEntity
{
    public GameManager GameManager { get; private set; }
    public AIManager AIManager { get; private set; }

    [AutoComponent]
    public MovavleGridObject GridObject { get; private set; }
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
    }

    private void UnRegister()
    {
        GameManager.Unregister(this);
        GameManager.AfterTimeChange -= AfterTimeChange;
    }

    private void AfterTimeChange(int prev, int current)
    {
        Refresh();
    }

    public void Refresh()
    {
        property = defaultProperty.Clone() as PawnProperty;
    }

    public void DoAction()
    {
        if(setting.humanControl)
        {
            //TODO
        }
        else
        {
            //TODO
        }
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