using Services;
using Services.ObjectPools;
using UnityEngine;

/// <summary>
/// 通过激活/回收MyObject来实现的动画，不应继承此类，应该继承AnimationObject
/// </summary>
public sealed class ObjectAnimationProcess : AnimationProcess
{
    private readonly IObjectManager objectManager;
    private MyObject myObject;

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

    public override float MockLatency(IAnimationSource source)
    {
        IMyObject obj = objectManager.Peek(prefabName);
        AnimationObject[] animationObjects = obj.Transform.GetComponentsInChildren<AnimationObject>();
        float max = 0f;
        for (int i = 0; i < animationObjects.Length; i++)
        {
            max = Mathf.Max(max, animationObjects[i].GetAnimationLatency(source));
        }
        return max;
    }

    //myObject被回收，标志着动画过程结束
    public void OneOffComplete()
    {
        Complete();
        myObject.OnRecycle -= OneOffComplete;   //回调仅生效一次
    }

    public override void Play()
    {
        myObject = objectManager.Activate(prefabName, position, Vector3.zero, parent) as MyObject;
        AnimationObject[] animationObjects = myObject.GetComponentsInChildren<AnimationObject>();
        for (int i = 0; i < animationObjects.Length; i++)
        {
            animationObjects[i].Initialize(source);
        }
        myObject.OnRecycle += OneOffComplete;
    }

    public override void Apply()
    {
        source.Apply();
    }
}
