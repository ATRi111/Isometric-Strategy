using Services;
using System.Collections;
using UnityEngine;

public class AnimationProcess
{
    protected AnimationManager manager;
    public Effect effect;

    public AnimationProcess(Effect effect)
    {
        manager = ServiceLocator.Get<AnimationManager>();
        this.effect = effect;
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
    public virtual void Play()
    {

    }
    /// <summary>
    /// 在动画结束后应用效果
    /// </summary>
    public virtual void Apply()
    {
        effect?.Apply();
    }
}

/// <summary>
/// 在固定时间后自动结束的动画过程
/// </summary>
public class AnimationProcess_FixedTime : AnimationProcess
{
    public float time;

    public AnimationProcess_FixedTime(Effect effect,float time)
        : base(effect)
    {
        this.time = time;
    }

    public override void Play()
    {
        base.Play();
        manager.StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(time);
        manager.Unregister(this);
    }
}