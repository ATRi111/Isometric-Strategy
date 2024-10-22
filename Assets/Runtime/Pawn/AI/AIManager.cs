using Services;
using System;
using UnityEngine;

public class AIManager : Service,IService
{
    public override Type RegisterType => GetType();

    [SerializeField]
    private PathFindingManager pathFinding;
    public PathFindingManager PathFinding => pathFinding;
}
