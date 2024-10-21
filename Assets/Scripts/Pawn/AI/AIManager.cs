using Services;
using System;
using UnityEngine;

public class AIManager : Service,IService
{
    public override Type RegisterType => GetType();

    [SerializeField]
    private PathFinding pathFinding;
    public PathFinding PathFinding => pathFinding;
}
