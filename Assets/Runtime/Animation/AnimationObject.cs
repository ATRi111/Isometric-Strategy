using Services.ObjectPools;
using System.Collections;
using UnityEngine;

/// <summary>
/// ���ڳ��ֶ���Ч����ʵ��
/// </summary>
[RequireComponent(typeof(MyObject))]
public abstract class AnimationObject : MonoBehaviour
{
    protected MyObject myObject;
    public float lifeSpan;

    public abstract void Activate(IAnimationSource source);

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
