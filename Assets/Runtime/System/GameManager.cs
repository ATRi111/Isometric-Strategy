using MyTool;
using Services;
using Services.Event;
using Services.SceneManagement;
using System;
using UnityEngine;

public class GameManager : Service,IService
{
    [AutoService]
    private AnimationManager animationManager;
    [AutoService]
    private IEventSystem eventSystem;
    [AutoService]
    private ISceneController sceneController;

    public int battleSceneIndex;

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

    public void EndBattle()
    {
        sceneController.UnloadScene(battleSceneIndex);
        battleSceneIndex++;
        sceneController.LoadScene(battleSceneIndex, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    private bool EnemyExists()
    {
        foreach(PawnEntity pawn in pawns)
        {
            if(pawn.faction == EFaction.Enemy) 
                return true;
        }
        return false;
    }

    public void MoveOn()
    {
        if (!EnemyExists())
        {
            EndBattle();
            return;
        }
        foreach(PawnEntity pawn in pawns)
        {
            if (time >= pawn.time)
            {
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
        battleSceneIndex = 3;
    }
}
