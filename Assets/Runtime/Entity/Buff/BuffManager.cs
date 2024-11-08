using Character;
using MyTool;
using Services;
using UnityEngine;

public class BuffManager : CharacterComponentBase
{
    private GameManager gameManager;
    [SerializeField]
    private SerializedHashSet<Buff> buffs;

    public void Register(Buff buff)
    {
        buffs.Add(buff);
        buff.OnEnable();
    }
    public void Unregister(Buff buff)
    {
        buffs.Remove(buff);
        buff.OnDisable();
    }
    public void Refresh(int currentTime)
    {
        foreach (Buff buff in buffs)
        {
            if(currentTime >= buff.endTime)
                Unregister(buff);
        }
    }
    private void OnTick(int time)
    {
        Refresh(time);
    }

    protected override void Awake()
    {
        base.Awake();
        gameManager = ServiceLocator.Get<GameManager>();
    }

    private void OnEnable()
    {
        gameManager.OnTick += OnTick;
        Refresh(gameManager.Time);
    }

    private void OnDisable()
    {
        gameManager.OnTick -= OnTick;
    }
}
