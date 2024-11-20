using Services;
using System;

public class AIManager : Service,IService
{
    private IsometricGridManager igm;
    private IsometricGridManager Igm
    {
        get
        {
            if (igm == null)
                igm = IsometricGridManager.FindInstance();
            return igm;
        }
    }

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
    public float offerHelp;
    public float seekHelp;
    public float charge;
    public float withdraw;
}