using Character;
using MyTimer;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : CharacterComponentBase
{
    public float defaultSpeed = 1f;
    [SerializeField]
    protected Vector3[] currentRoute;
    [SerializeField]
    protected UniformFoldLineMotion ufm;

    public Action AfterMove;

    public Vector3 Position
    {
        get => entity.transform.position;
        set => entity.transform.position = value;
    }

    protected override void Awake()
    {
        base.Awake(); 
        ufm = new UniformFoldLineMotion();
        ufm.OnTick += OnTick;
        ufm.AfterComplete += AfterComplete;
    }

    protected virtual void OnDisable()
    {
        ufm.Paused = true;
    }

    public void SetRoute(List<Vector3> route, float speed)
    {
        if (route.Count < 2)
            return;
        currentRoute = route.ToArray();
        float length = 0;
        for (int i = 1; i < currentRoute.Length; i++)
        {
            length += (currentRoute[i] - currentRoute[i - 1]).magnitude;
        }
        ufm.Initialize(currentRoute, length, length / speed);
    }

    protected virtual void OnTick(Vector3 v)
    {
        Position = v;
    }

    protected virtual void AfterComplete(Vector3 v)
    {
        Position = v;
        AfterMove?.Invoke();
    }

    public virtual void ForceComplete()
    {
        ufm.ForceComplete();
    }
}
