using Character;
using Services;
using UnityEngine;

public class PawnEntity : CharacterEntity
{
    private GameManager gameManager;

    [AutoComponent]
    public GridPawn GridPawn { get; private set; }
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
        gameManager = ServiceLocator.Get<GameManager>();
    }

    private void OnEnable()
    {
        Initialize();
        Register();
    }

    private void LateUpdate()
    {
        Refresh();
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

    public void Refresh()
    {
        property = defaultProperty.Clone() as PawnProperty;
    }

    private void Register()
    {
        gameManager.Register(this);
    }

    private void UnRegister()
    {
        gameManager.Unregister(this);
    }

    public void DoAction()
    {

    }
}