using Services;
using Services.Audio;
using Services.Event;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : AudioPlayerBase
{
    [SerializeField]
    private AudioMixer audioMixer;
    [AutoService]
    private IEventSystem eventSystem;

    private AudioSource currentBgm;
    public AudioPlayList battleBgmList;

    public void SetVolume(string parameter, float volume)
    {
        volume = Mathf.Clamp(volume, 0.01f, 1f);
        core.SetVolume(audioMixer, parameter, volume);
    }

    private void BeforeBattle()
    {
        battleBgmList.Play();
    }

    private void AfterBattle(bool win)
    {
        battleBgmList.Pause();
    }

    protected internal override void Init()
    {
        base.Init();
        eventSystem.AddListener(EEvent.BeforeBattle, BeforeBattle);
        eventSystem.AddListener<bool>(EEvent.AfterBattle, AfterBattle);
    }

    private void OnDestroy()
    {
        eventSystem.RemoveListener(EEvent.BeforeBattle, BeforeBattle);
        eventSystem.RemoveListener<bool>(EEvent.AfterBattle, AfterBattle);
    }
}
