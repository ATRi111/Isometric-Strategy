using Services;
using Services.ObjectPools;
using UnityEngine;

/// <summary>
/// 通过激活/回收MyObject来实现的效果附带的动画
/// </summary>
public class EffectObjectAnimationProcess : EffectAnimationProcess
{
    protected readonly IObjectManager objectManager;
    protected MyObject myObject;

    public string prefabName;
    public Transform parent;
    public Vector3 position;

    public EffectObjectAnimationProcess(Effect effect, string prefabName, Transform parent, Vector3 position)
        : base(effect)
    {
        objectManager = ServiceLocator.Get<IObjectManager>();
        this.prefabName = prefabName;
        this.parent = parent;
        this.position = position;
    }

    public void OneOffComplete()
    {
        Complete();
        myObject.OnRecycle -= OneOffComplete;   //回调仅生效一次
    }

    public override void Play()
    {
        myObject = objectManager.Activate(prefabName, position, Vector3.zero, parent) as MyObject;
        myObject.OnRecycle += OneOffComplete;
    }
}
