using Services;
using System.Collections;
using UnityEngine;

public abstract class AnimationProcess
{
    protected AnimationManager manager;
    public bool completed;

    public AnimationProcess()
    {
        manager = ServiceLocator.Get<AnimationManager>();
        completed = false;
    }

    /// <summary>
    /// 在一定延迟后播放动画
    /// </summary>
    /// <param name="latency"></param>
    public virtual void Play(float latency)
    {
        if (latency == 0f)
            Play();
        else
            manager.StartCoroutine(DelayPlay(latency));
    }

    private IEnumerator DelayPlay(float latency)
    {
        yield return new WaitForSeconds(latency);
        Play();
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    public abstract void Play();
    /// <summary>
    /// 动画播放结束
    /// </summary>
    public virtual void Complete()
    {
        completed = true;
        manager.Unregister(this);
    }

    /// <summary>
    /// 在动画结束后应用效果
    /// </summary>
    public abstract void Apply();
}