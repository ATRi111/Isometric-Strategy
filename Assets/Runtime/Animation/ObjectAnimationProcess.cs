using Services;
using Services.ObjectPools;
using UnityEngine;

/// <summary>
/// ͨ������/����MyObject��ʵ�ֵĶ���
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

    public void OneOffComplete()
    {
        Complete();
        myObject.OnRecycle -= OneOffComplete;   //�ص�����Чһ��
    }

    public override void Play()
    {
        myObject = objectManager.Activate(prefabName, position, Vector3.zero, parent) as MyObject;
        if (myObject.TryGetComponent(out AnimationObject animationObject))
        {
            animationObject.Activate(source);
        }
        myObject.OnRecycle += OneOffComplete;
    }

    public override void Apply()
    {
        
    }
}
