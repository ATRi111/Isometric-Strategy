using MyTool;
using Services;
using System;
using UnityEngine;

public class GameManager : Service,IService
{
    [AutoService]
    private AnimationManager animationManager;

    public override Type RegisterType => GetType();

    public SerializedHashSet<PawnEntity> pawns = new();

    public Action<PawnEntity, int> BeforeDoAction;
    public Action BeforeBattle;
    public Action<int> OnTick;

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
        BeforeBattle?.Invoke();
        MoveOn();
    }

    public void MoveOn()
    {
        //TODO:Õ½¶·½áÊøÅÐ¶¨
        foreach(PawnEntity pawn in pawns)
        {
            if (time >= pawn.time)
            {
                BeforeDoAction?.Invoke(pawn, time);
                waitingForAnimation = true;
                pawn.Brain.DoAction();
                return;
            }
        }
        time++;
        OnTick?.Invoke(time);
        waitingForAnimation = true;
        animationManager.StartAnimationCheck();
    }

    private void AfterAnimationComplete()
    {
        if (!waitingForAnimation)
            throw new InvalidOperationException();
        Debug.Log("Animation Complete");
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
