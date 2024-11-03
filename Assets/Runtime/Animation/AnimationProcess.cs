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
    /// ���Ŷ���
    /// </summary>
    public virtual void Play()
    {

    }
    /// <summary>
    /// �ڶ���������Ӧ��Ч��
    /// </summary>
    public virtual void Apply()
    {
        effect?.Apply();
    }
}

/// <summary>
/// �ڹ̶�ʱ����Զ������Ķ�������
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