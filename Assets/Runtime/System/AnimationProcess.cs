using System;
using System.Collections;
using UnityEngine;

public class AnimationProcess
{
    protected AnimationManager manager;
    public Action OnPlay;
    public Action AfterComplete;
    
    public AnimationProcess(AnimationManager manager)
    {
        this.manager = manager;
    }
}

/// <summary>
/// 在固定时间后自动结束的动画过程
/// </summary>
public class AnimationProcess_FixedTime : AnimationProcess
{
    public float time;

    public AnimationProcess_FixedTime(AnimationManager manager, float time)
        :base(manager)
    {
        this.time = time;
        OnPlay += StartDelay;
    }

    private void StartDelay()
    {
        manager.StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(time);
        manager.Unregister(this);
    }
}