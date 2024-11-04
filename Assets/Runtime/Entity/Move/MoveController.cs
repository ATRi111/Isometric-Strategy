using Character;
using MyTimer;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : CharacterComponentBase
{
    [SerializeField]
    protected float speed = 1f;
    [SerializeField]
    protected Vector3[] currentRoute;
    [SerializeField]
    protected int currentIndex;
    [SerializeField]
    protected UniformLinearMotion ulm;

    public Action AfterMove;

    public Vector3 Position
    {
        get => entity.transform.position;
        set => entity.transform.position = value;
    }

    protected override void Awake()
    {
        base.Awake(); 
        ulm = new UniformLinearMotion();
        ulm.OnTick += OnTick;
        ulm.AfterCompelete += AfterComplete;
    }

    protected virtual void OnDisable()
    {
        ulm.Paused = true;
    }

    public void SetRoute(List<Vector3> route)
    {
        if (route.Count < 2)
            return;
        currentRoute = route.ToArray();
        currentIndex = 1;
        MoveTo(currentRoute[currentIndex]);
    }

    protected virtual void OnTick(Vector3 v)
    {
        Position = v;
    }

    protected virtual void AfterComplete(Vector3 v)
    {
        Position = v;
        currentIndex++;
        if (currentIndex < currentRoute.Length)
            MoveTo(currentRoute[currentIndex]);
        else
            AfterMove?.Invoke();
    }
    public virtual void ForceComplete()
    {
        ulm.Paused = true;
        Position = currentRoute[^1];
        AfterMove?.Invoke();
    }

    protected virtual void MoveTo(Vector3 target)
    {
        ulm.Initialize(Position, target, (target - Position).magnitude / speed);
    }
}
