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
    [NonSerialized]
    public bool inBattle;

    public override Type RegisterType => GetType();

    public SerializedHashSet<PawnEntity> pawns = new();

    [SerializeField]
    private int time;
    public int Time => time;

    public bool waitingForAnimation;
    public bool allyTargetDied;

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
        if (entity.taskTarget && entity.faction == EFaction.Ally)
            allyTargetDied = true;
    }

    public void StartBattle()
    {
        time = 0;
        inBattle = true;
        eventSystem.Invoke(EEvent.BeforeBattle);
        MoveOn();
    }

    public void EndBattle(bool win)
    {
        inBattle = false;
        eventSystem.Invoke(EEvent.AfterBattle, win);
    }

    public void LoadNextLevel()
    {
        sceneController.UnloadScene(battleSceneIndex);
        battleSceneIndex++;
        sceneController.LoadScene(battleSceneIndex, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    private bool EnemyTargetExists()
    {
        foreach(PawnEntity pawn in pawns)
        {
            if (pawn.taskTarget && pawn.faction == EFaction.Enemy && pawn.gameObject.activeInHierarchy) 
                return true;
        }
        return false;
    }

    public void MoveOn()
    {
        if (allyTargetDied)
        {
            EndBattle(false);
            allyTargetDied = false;
            return;
        }
        if (!EnemyTargetExists())
        {
            EndBattle(true);
            return;
        }
        foreach(PawnEntity pawn in pawns)
        {
            if (time >= pawn.time)
            {
                Debug.Log($"ÂÖµ½{pawn.EntityNameWithColor}ÐÐ¶¯");
                waitingForAnimation = true;
                pawn.Brain.DoAction();
                return;
            }
        }
        time++;
        eventSystem.Invoke(EEvent.OnTick, time);
        waitingForAnimation = true;
        animationManager.CheckAnimation();
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
}
