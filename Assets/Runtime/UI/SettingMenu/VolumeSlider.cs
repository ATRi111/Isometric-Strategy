using Services;
using Services.Audio;
using UIExtend;
using UnityEngine;

public class VolumeSlider : SliderBase<float>
{
    private AudioPlayer audioPlayer;
    [SerializeField]
    private string parameter;

    protected override void OnValueChanged(float value)
    {
        audioPlayer.SetVolume(parameter, ValueToData(value));    
    }

    protected override float DataToValue(float data)
    {
        return data;
    }
    protected override float ValueToData(float value)
    {
        return value;
    }

    protected override void Awake()
    {
        base.Awake();
        audioPlayer = ServiceLocator.Get<IAudioPlayer>() as AudioPlayer;
    }
}
