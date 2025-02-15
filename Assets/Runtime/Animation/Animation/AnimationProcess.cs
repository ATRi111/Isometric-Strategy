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
    /// Ԥ���㲥�Ŵ˶�����Ҫ��ʱ��
    /// </summary>
    public abstract float MockTime(IAnimationSource source);

    /// <summary>
    /// ��һ���ӳٺ󲥷Ŷ���
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