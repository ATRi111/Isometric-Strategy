using MyTimer;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public static float CalculateLength(List<Vector3> route)
    {
        if (route.Count < 2)
            return 0f;
        IsometricGridManager igm = IsometricGridManager.Instance;
        Vector3 prev = route[0];
        Vector3 current;
        float length = 0;
        for (int i = 1; i < route.Count; i++)
        {
            current = igm.CellToWorld(route[i]);
            length += (current - prev).magnitude;
            prev = current;
        }
        return length;
    }

    public float defaultSpeed = 1f;
    [SerializeField]
    protected Vector3[] currentRoute;
    [SerializeField]
    protected UniformFoldLineMotion ufm;

    public Action AfterMove;
    protected Transform target;

    public Vector3 Position
    {
        get => target.position;
        set => target.position = value;
    }

    public float MockTime(List<Vector3> route, float speed)
    {
        return CalculateLength(route) / speed;
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

    protected virtual void Awake()
    {
        ufm = new UniformFoldLineMotion();
        ufm.OnTick += OnTick;
        ufm.AfterComplete += AfterComplete;
        target = transform;
    }

    protected virtual void OnDisable()
    {
        ufm.Paused = true;
    }
}
