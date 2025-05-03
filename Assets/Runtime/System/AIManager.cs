using Services;
using System;
using UnityEngine;

public class AIManager : Service, IService
{
    public override Type RegisterType => GetType();

    private PathFindingManager pathFinding;
    public PathFindingManager PathFinding => pathFinding;

    public Trend trend;

    protected override void Awake()
    {
        base.Awake();
        pathFinding = GetComponent<PathFindingManager>();
    }
}

[Serializable]
public struct Trend
{
    public float offerSupport;
    public float seekSupport;
    public float offense;
    public float defense;
    public float terrain;

    public readonly float Multiply(float a, float b, float c, float d)
    {
        return offerSupport * a + seekSupport * b + offense * c + defense * d;
    }

    public static Trend operator +(Trend a, Trend b)
    {
        Trend ret = new()
        {
            offerSupport = a.offerSupport + b.offerSupport,
            seekSupport = a.seekSupport + b.seekSupport,
            offense = a.offense + b.offense,
            defense = a.defense + b.defense,
            terrain = a.terrain,
        };
        return ret;
    }
}

public class PNorm
{
    public float p;
    public float sum;

    public float Result => Mathf.Pow(sum, 1 / p);

    public PNorm(float p)
    {
        this.p = p;
    }

    public void Add(float value)
    {
        sum += Mathf.Pow(value, p);
    }
}