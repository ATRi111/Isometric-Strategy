using Services;
using Services.Audio;
using Services.ObjectPools;
using System.Collections;
using UnityEngine;

/// <summary>
/// 动画脚本可能有多个，应当令生命周期最长的脚本继承此类，以控制回收，其他脚本持有此类
/// </summary>
[RequireComponent(typeof(MyObject))]
public abstract class AnimationObject : MonoBehaviour
{
    protected IsometricGridManager igm;
    protected MyObject myObject;
    protected IAudioPlayer audioPlayer;
    protected IObjectManager objectManager;

    public GameObject audio_activate;
    public GameObject audio_recycle;
    public float lifeSpan;
    public IAnimationSource source;

    public virtual void Initialize(IAnimationSource source)
    {
        this.source = source;
        StartCoroutine(DelayRecycle(lifeSpan));
        if (audio_activate != null)
            audioPlayer.CreateAudioByPrefab(audio_activate.name, transform.position);
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
        if (audio_recycle != null)
            audioPlayer.CreateAudioByPrefab(audio_recycle.name, transform.position);
        source = null;
        ObjectPoolUtility.RecycleMyObjects(gameObject);
        myObject.Recycle();
    }

    protected virtual void Awake()
    {
        igm = IsometricGridManager.Instance;
        myObject = GetComponent<MyObject>();
        audioPlayer = ServiceLocator.Get<IAudioPlayer>();
        objectManager = ServiceLocator.Get<IObjectManager>();
    }
}
