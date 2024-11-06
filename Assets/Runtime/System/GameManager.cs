using Services;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Service,IService
{
    [AutoService]
    private AnimationManager animationManager;

    public override Type RegisterType => GetType();
    [SerializeField]
    private SerializedHashSet<PawnEntity> pawns = new();

    public Action<PawnEntity, int> BeforeDoAction;
    public Action OnStartBattle;

    [SerializeField]
    private int time;
    public int Time => time;

    public List<PawnAction> actionList;

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
        OnStartBattle?.Invoke();
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
                pawn.Brain.DoAction();
                animationManager.AfterNoAnimation += AfterAnimationComplete;
                return;
            }
        }
        time++;
        MoveOn();
    }

    private void AfterAnimationComplete()
    {
        animationManager.AfterNoAnimation -= AfterAnimationComplete;
        MoveOn();
    }
}
