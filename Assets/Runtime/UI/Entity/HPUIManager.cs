using Services;
using Services.Event;
using Services.ObjectPools;
using UnityEngine;

[DefaultExecutionOrder(-200)]
public class HPUIManager : MonoBehaviour
{
    private IEventSystem eventSystem;
    private IObjectManager objectManager;

    private void AfterEntityEnable(Entity entity)
    {
        IMyObject myObject = objectManager.Activate(nameof(FollowingHPBar), entity.transform.position, Vector3.zero, transform);
        FollowingHPBar HPBar = myObject.Transform.GetComponent<FollowingHPBar>();
        HPBar.SetEntity(entity);
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        objectManager = ServiceLocator.Get<IObjectManager>();
    }

    private void OnEnable()
    {
        eventSystem.AddListener<Entity>(EEvent.AfterEntityEnable, AfterEntityEnable);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<Entity>(EEvent.AfterEntityEnable, AfterEntityEnable);
    }
}
