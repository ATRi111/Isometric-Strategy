using EditorExtend.GridEditor;
using Services;
using Services.Event;

public class SpawnPoint : GridObject
{
    private IEventSystem eventSystem;
    /// <summary>
    /// �����ڼ���״̬��ʾ�˴�������������ҽ�ɫ
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
        eventSystem = ServiceLocator.Get<IEventSystem>();
        eventSystem.AddListener(EEvent.BeforeBattle,BeforeBattle);
    }

    private void OnDestroy()
    {
        eventSystem.RemoveListener(EEvent.BeforeBattle, BeforeBattle);
    }
}
