using Services;
using Services.Audio;
using Services.ObjectPools;
using System.Collections;
using UnityEngine;

/// <summary>
/// 一个物体上可以有多个此脚本，均播放完毕后回收物体
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
    public IAnimationSource source;
    public float lifeSpan = 1f;
    public float nomalizedLatency = 1f;

    public virtual void Initialize(IAnimationSource source)
    {
        this.source = source;
        myObject.RecycleCount++;
        StartCoroutine(DelayRecycle(lifeSpan));
        if (audio_activate != null)
            audioPlayer.CreateAudioByPrefab(audio_activate.name, transform.position);
    }

    /// <summary>
    /// 预计算延迟（即后续动画的等待时间），可以小于lifeSpan（表示动画未播放完时即衔接后续动画）
    /// </summary>
    public virtual float GetAnimationLatency(IAnimationSource source)
    {
        return lifeSpan * nomalizedLatency;
    }

    protected IEnumerator DelayRecycle(float t)
    {
        if (t > 0)
            yield return new WaitForSeconds(t);
        else
            yield return new WaitForEndOfFrame();

        if (audio_recycle != null)
            audioPlayer.CreateAudioByPrefab(audio_recycle.name, transform.position);
        source = null;
        if (myObject.RecycleCount == 1)
            ObjectPoolUtility.RecycleMyObjects(gameObject);
        myObject.RecycleCount--;
    }

    protected virtual void Awake()
    {
        igm = IsometricGridManager.Instance;
        myObject = GetComponent<MyObject>();
        audioPlayer = ServiceLocator.Get<IAudioPlayer>();
        objectManager = ServiceLocator.Get<IObjectManager>();
    }
}
