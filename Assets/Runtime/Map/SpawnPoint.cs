using EditorExtend.GridEditor;
using Services;
using Services.Event;

public class SpawnPoint : GridObject
{
    /// <summary>
    /// 自身处于激活状态表示此处可用于生成玩家角色
    /// </summary>
    public bool IsEmpty
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }

    public void BeforeBattle()
    {
        gameObject.SetActive(false);
    }

    protected override void Awake()
    {
        base.Awake();
        IEventSystem eventSystem = ServiceLocator.Get<IEventSystem>();
        eventSystem.AddListener(EEvent.BeforeBattle,BeforeBattle);
    }
}
