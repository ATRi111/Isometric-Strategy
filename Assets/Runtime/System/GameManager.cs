using MyTool;
using Services;
using Services.Event;
using System;
using UnityEngine;

public class GameManager : Service,IService
{
    [AutoService]
    private AnimationManager animationManager;
    [AutoService]
    private IEventSystem eventSystem;

    public override Type RegisterType => GetType();

    public SerializedHashSet<PawnEntity> pawns = new();

    [SerializeField]
    private int time;
    public int Time => time;

    public bool waitingForAnimation;

#if UNITY_EDITOR
    public bool debug;
#endif

    public void Register(PawnEntity entity)
    {
        pawns.Add(entity);
    }
    public void Unregister(PawnEntity entity)
    {
        pawns.Remove(entity);
    }

    public void StartBattle()
    {
        time = 0;
        eventSystem.Invoke(EEvent.BeforeBattle);
        MoveOn();
    }

    public void MoveOn()
    {
        //TODO:Õ½¶·½áÊøÅÐ¶¨
        foreach(PawnEntity pawn in pawns)
        {
            if (time >= pawn.time)
            {
                eventSystem.Invoke(EEvent.BeforeDoAction, pawn, time);
                waitingForAnimation = true;
                pawn.Brain.DoAction();
                return;
            }
        }
        time++;
        eventSystem.Invoke(EEvent.OnTick, time);
        waitingForAnimation = true;
        animationManager.StartAnimationCheck();
    }

    private void AfterAnimationComplete()
    {
        if (!waitingForAnimation)
            throw new InvalidOperationException();
        waitingForAnimation = false;
        MoveOn();
    }

    protected internal override void Init()
    {
        base.Init();
        animationManager.AfterAnimationComplete += AfterAnimationComplete;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.K))
        {
            StartBattle();
        }
    }
}
