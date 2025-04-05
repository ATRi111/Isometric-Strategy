using Services;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
public abstract class AnimationProcess
{
    protected AnimationManager manager;
    public string name;
    public bool completed;
    [NonSerialized]
    public AnimationProcess joinedAnimation;

    public AnimationProcess()
    {
        manager = ServiceLocator.Get<AnimationManager>();
        completed = false;
        name = GetType().ToString();
    }

    /// <summary>
    /// 预计算延迟（即后续动画的等待时间）
    /// </summary>
    public abstract float MockLatency(IAnimationSource source);

    /// <summary>
    /// 在一定延迟后播放动画
    /// </summary>
    /// <param name="latency"></param>
    public virtual void Play(float latency)
    {
        manager.StartCoroutine(DelayPlay(latency));
    }

    protected virtual IEnumerator DelayPlay(float latency)
    {
        if (joinedAnimation != null)
        {
            while (!joinedAnimation.completed)
            {
                yield return null;
            }
        }
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