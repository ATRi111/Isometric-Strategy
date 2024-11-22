using Character;
using Services;
using Services.Event;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : CharacterComponentBase
{
    private PawnEntity pawn;
    private IEventSystem eventSystem;
    private GameManager gameManager;
    [SerializeField]
    private List<RuntimeBuff> buffs;

    public void OnTick(int time)
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            buffs[i].Enabled = gameManager.Time < buffs[i].endTime; 
            if (buffs[i].Enabled)
                buffs[i].Tick(time);
        }
    }

    public void Add(Buff buff)
    {
        //TODO:同名Buff处理
        RuntimeBuff runtimeBuff = new(pawn, buff, gameManager.Time);
        buffs.Add(runtimeBuff);
        runtimeBuff.Enabled = true;
    }

    public void Remove(Buff buff,int startTime)
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            if(buff.buffName == buffs[i].buffName && startTime == buffs[i].startTime)
            {
                buffs.RemoveAt(i);
            }
        }
    }

    private void AfterBattle()
    {
        buffs.Clear();
    }

    protected override void Awake()
    {
        base.Awake();
        eventSystem = ServiceLocator.Get<IEventSystem>();
        gameManager = ServiceLocator.Get<GameManager>();
        pawn = entity as PawnEntity;
    }

    private void OnEnable()
    {
        eventSystem.AddListener(EEvent.AfterBattle, AfterBattle);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener(EEvent.AfterBattle, AfterBattle);
    }
}
