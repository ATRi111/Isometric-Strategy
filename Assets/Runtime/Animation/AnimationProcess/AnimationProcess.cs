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
    /// Ԥ�����ӳ٣������������ĵȴ�ʱ�䣩
    /// </summary>
    public abstract float MockLatency(IAnimationSource source);

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