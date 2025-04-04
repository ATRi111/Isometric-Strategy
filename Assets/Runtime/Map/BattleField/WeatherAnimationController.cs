using Services;
using Services.Event;
using System.Collections.Generic;
using UnityEngine;

public class WeatherAnimation
{
    public EWeather weather;
    public GameObject[] gameObjects;
    public ParticleSystem[] particleSystems;

    public void Play()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(true);
        }
        for (int i = 0;i < particleSystems.Length; i++)
        {
            particleSystems[i].Play();
        }
    }

    public void Stop()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(false);
        }
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Stop();
        }
    }
}

public class WeatherAnimationController : MonoBehaviour
{
    private IEventSystem eventSystem;

    private Dictionary<EWeather, WeatherAnimation> seacher;
    public WeatherAnimation[] weatherAnimations;

    private void AfterWeatherChange(BattleField battleField)
    {
        foreach(WeatherAnimation animation in seacher.Values)
        {
            if(animation.weather == battleField.Weather)
                animation.Play();
            else
                animation.Stop();
        }
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        seacher = new();
        for (int i = 0; i < weatherAnimations.Length; i++)
        {
            seacher.Add(weatherAnimations[i].weather,weatherAnimations[i]);
        }
    }

    private void OnEnable()
    {
        eventSystem.AddListener<BattleField>(EEvent.AfterWeatherChange, AfterWeatherChange);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<BattleField>(EEvent.AfterWeatherChange, AfterWeatherChange);
    }
}
