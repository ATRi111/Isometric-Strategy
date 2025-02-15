using Services.ObjectPools;
using System.Collections;
using UnityEngine;

/// <summary>
/// 用于呈现动画效果的实体
/// </summary>
[RequireComponent(typeof(MyObject))]
public abstract class AnimationObject : MonoBehaviour
{
    protected IsometricGridManager Igm => IsometricGridManager.Instance;

    protected MyObject myObject;
    [SerializeField]
    protected float lifeSpan;

    public virtual void Initialize(IAnimationSource source)
    {
        StartCoroutine(DelayRecycle(lifeSpan));
    }

    /// <summary>
    /// 预计算延迟（即后续动画的等待时间），可以小于lifeSpan（表示动画未播放完时即衔接后续动画）
    /// </summary>
    public virtual float GetAnimationLatency(IAnimationSource source)
    {
        return lifeSpan;
    }

    protected IEnumerator DelayRecycle(float t)
    {
        yield return new WaitForSeconds(t);
        myObject.Recycle();
    }

    protected virtual void Awake()
    {
        myObject = GetComponent<MyObject>();
    }
}
