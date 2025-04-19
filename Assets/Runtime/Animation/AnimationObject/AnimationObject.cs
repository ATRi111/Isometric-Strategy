using Services;
using Services.Audio;
using Services.ObjectPools;
using System.Collections;
using UnityEngine;

/// <summary>
/// һ�������Ͽ����ж���˽ű�����������Ϻ��������
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
    /// Ԥ�����ӳ٣������������ĵȴ�ʱ�䣩������С��lifeSpan����ʾ����δ������ʱ���νӺ���������
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
