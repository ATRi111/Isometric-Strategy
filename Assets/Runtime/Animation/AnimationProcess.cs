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
    /// ��һ���ӳٺ󲥷Ŷ���
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
    /// ���Ŷ���
    /// </summary>
    public abstract void Play();
    /// <summary>
    /// �������Ž���
    /// </summary>
    public virtual void Complete()
    {
        completed = true;
        manager.Unregister(this);
    }

    /// <summary>
    /// �ڶ���������Ӧ��Ч��
    /// </summary>
    public abstract void Apply();
}