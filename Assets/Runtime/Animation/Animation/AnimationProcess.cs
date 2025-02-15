using Services;
using System.Collections;
using UnityEngine;

public abstract class AnimationProcess
{
    protected AnimationManager manager;
    public AnimationProcess joinedAnimation;
    public bool completed;

    public AnimationProcess()
    {
        manager = ServiceLocator.Get<AnimationManager>();
        completed = false;
    }

    /// <summary>
    /// 预计算播放此动画需要的时间
    /// </summary>
    public abstract float MockTime(IAnimationSource source);

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
        yield return new WaitForSeconds(latency);
        if (joinedAnimation != null)
        {
            while (!joinedAnimation.completed)
            {
                yield return null;
            }
        }
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