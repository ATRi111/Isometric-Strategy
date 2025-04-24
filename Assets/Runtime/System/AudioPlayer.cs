using Services.Audio;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : AudioPlayerBase
{
    [SerializeField]
    private AudioMixer audioMixer;

    public void SetVolume(string parameter, float volume)
    {
        volume = Mathf.Clamp(volume, 0.01f, 1f);
        core.SetVolume(audioMixer, parameter, volume);
    }
}
