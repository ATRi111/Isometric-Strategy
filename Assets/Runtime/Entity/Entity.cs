using Character;
using EditorExtend.GridEditor;
using Services;
using Services.Event;
using UnityEngine;

public class Entity : EntityBase
{
    public IsometricGridManager Igm => IsometricGridManager.Instance;
    public GameManager GameManager { get; protected set; }
    public AnimationManager AnimationManager { get; protected set; }
    public IEventSystem EventSystem { get; protected set; }
    [AutoComponent]
    public GridObject GridObject { get; protected set; }
    [AutoComponent]
    public GridMoveController MoveController { get; protected set; }
    [AutoComponent]
    public DefenceComponent DefenceComponent { get; protected set; }

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
        DefenceComponent.Refresh();
    }

    protected virtual void OnTick(int time)
    {
        RefreshProperty();
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
    }

    protected virtual void OnEnable()
    {
        EventSystem.AddListener<int>(EEvent.OnTick, OnTick);
        DefenceComponent.Initialize();
        RefreshProperty();
        DefenceComponent.HP = DefenceComponent.maxHP.IntValue;
    }

    protected virtual void OnDisable()
    {
        EventSystem.RemoveListener<int>(EEvent.OnTick, OnTick);
    }
}
