using Services;
using Services.Audio;
using Services.ObjectPools;
using System.Collections;
using UnityEngine;

/// <summary>
/// �����ű������ж����Ӧ��������������Ľű��̳д��࣬�Կ��ƻ��գ������ű����д���
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
    /// Ԥ�����ӳ٣������������ĵȴ�ʱ�䣩������С��lifeSpan����ʾ����δ������ʱ���νӺ���������
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
