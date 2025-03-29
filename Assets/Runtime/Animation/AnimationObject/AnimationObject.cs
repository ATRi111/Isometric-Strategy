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

    public string audio_activate;
    public string audio_recycle;
    public float lifeSpan;
    public IAnimationSource source;

    public virtual void Initialize(IAnimationSource source)
    {
        this.source = source;
        StartCoroutine(DelayRecycle(lifeSpan));
        if (!string.IsNullOrWhiteSpace(audio_activate))
            audioPlayer.CreateAudioByPrefab(audio_activate, transform.position);
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
        if (!string.IsNullOrWhiteSpace(audio_recycle))
            audioPlayer.CreateAudioByPrefab(audio_recycle, transform.position);
        source = null;
        myObject.Recycle();
    }

    protected virtual void Awake()
    {
        igm = IsometricGridManager.Instance;
        myObject = GetComponent<MyObject>();
        audioPlayer = ServiceLocator.Get<IAudioPlayer>();
        objectManager = ServiceLocator.Get<IObjectManager>();
    }

    private void OnDisable()
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
    }
}
