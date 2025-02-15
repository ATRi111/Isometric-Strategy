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

    public abstract void Activate(IAnimationSource source);

    public virtual float CalculateLifeSpan(IAnimationSource source)
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
