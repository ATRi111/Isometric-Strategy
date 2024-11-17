using Character;
using Services;
using Services.Event;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : CharacterComponentBase
{
    private IEventSystem eventSystem;
    private GameManager gameManager;
    [SerializeField]
    private List<RuntimeBuff> buffs;

    public void Refresh(int time)
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            buffs[i].Enabled = gameManager.Time < buffs[i].endTime; 
            if (buffs[i].Enabled)
                buffs[i].Tick(time);
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
