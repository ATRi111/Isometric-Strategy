using Services;
using System;

public class AIManager : Service,IService
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

    public readonly float Multiply(float a, float b, float c, float d)
    {
        return offerSupport * a + seekSupport * b + offense * c + defense * d;
    }
}