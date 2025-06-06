﻿using MyTool;
using Services;
using Services.Event;
using Services.Save;
using Services.SceneManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Service, IService
{
    [AutoService]
    private AnimationManager animationManager;
    [AutoService]
    private IEventSystem eventSystem;
    [AutoService]
    private ISceneController sceneController;
    [AutoService]
    private ISaveManager saveManager;
    private SaveGroupController defaultGroup;

    public int battleSceneIndex;
    [NonSerialized]
    public bool inBattle;


    public override Type RegisterType => GetType();

    public SerializedHashSet<PawnEntity> pawns = new();
    public List<PawnEntity> sortedPawns = new();
    private readonly Comparer_PawnEntity_ActionTime comparer = new();

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
        eventSystem.Invoke(EEvent.OnTick, time);
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
        defaultGroup.Save();
        sceneController.LoadScene(battleSceneIndex, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    private bool EnemyTargetExists()
    {
        foreach (PawnEntity pawn in pawns)
        {
            if (pawn.taskTarget && pawn.faction == EFaction.Enemy && pawn.gameObject.activeInHierarchy)
                return true;
        }
        return false;
    }

    private void Sort()
    {
        sortedPawns.Clear();
        sortedPawns.AddRange(pawns.list);
        sortedPawns.Sort(comparer);
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
        Sort();
        for (int i = 0; i < sortedPawns.Count; i++)
        {
            PawnEntity pawn = sortedPawns[i];
            if (time >= pawn.time)
            {
                Debug.Log($"轮到{pawn.EntityNameWithColor}行动");
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
        defaultGroup = saveManager.GetGroup(1);
    }
}
