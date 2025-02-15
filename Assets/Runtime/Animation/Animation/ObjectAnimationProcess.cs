using Services;
using Services.ObjectPools;
using UnityEngine;

/// <summary>
/// 通过激活/回收MyObject来实现的动画
/// </summary>
public class ObjectAnimationProcess : AnimationProcess
{
    protected readonly IObjectManager objectManager;
    protected MyObject myObject;

    public IAnimationSource source;
    public string prefabName;
    public Transform parent;
    public Vector3 position;

    public ObjectAnimationProcess(IAnimationSource source, string prefabName, Transform parent, Vector3 position)
        : base()
    {
        objectManager = ServiceLocator.Get<IObjectManager>();
        this.source = source;
        this.prefabName = prefabName;
        this.parent = parent;
        this.position = position;
    }

    public override float MockTime(IAnimationSource source)
    {
        IMyObject obj = objectManager.Peek(prefabName);
        AnimationObject animationObject = obj.Transform.GetComponent<AnimationObject>();
        return animationObject.CalculateLifeSpan(source);
    }

    public void OneOffComplete()
    {
        Complete();
        myObject.OnRecycle -= OneOffComplete;   //回调仅生效一次
    }

    public override void Play()
    {
        myObject = objectManager.Activate(prefabName, position, Vector3.zero, parent) as MyObject;
        AnimationObject animationObject = myObject.GetComponent<AnimationObject>();
        animationObject.Activate(source);
        myObject.OnRecycle += OneOffComplete;
    }

    public override void Apply()
    {
        source.Apply();
    }
}
