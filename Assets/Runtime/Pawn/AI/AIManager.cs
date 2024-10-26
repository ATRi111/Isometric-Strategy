using Services;
using System;

public class AIManager : Service,IService
{
    public override Type RegisterType => GetType();

    private PathFindingManager pathFinding;
    public PathFindingManager PathFinding => pathFinding;

    protected override void Awake()
    {
        base.Awake();
        pathFinding = GetComponent<PathFindingManager>();
    }

    public void MakePlan()
    {

    }
}
