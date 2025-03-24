using Services;
using Services.Asset;
using Services.Event;
using System;
using UnityEngine;

public class BattleField : MonoBehaviour
{
    public static int WeatherDuration = 250;

    private IEventSystem eventSystem;
    private GameManager gameManager;

    public WeatherSettings weatherSettings;
    public Action<EWeather> AfterWeatherChange;

    public int nextResetTime;

    [SerializeField]
    private EWeather weather;
    public EWeather Weather
    {
        get => weather;
        set
        {
            if(weather != value)
            {
                weather = value;
                if(value != EWeather.None)
                    nextResetTime = gameManager.Time + WeatherDuration;
                AfterWeatherChange?.Invoke(weather);
            }
        }
    }

    /// <summary>
    /// 当前天气的剩余时间
    /// </summary>
    public int RemainingTime
    {
        get
        {
            if(weather == EWeather.None)
                return 0;
            return nextResetTime - gameManager.Time;
        }
    }

    public float MockPowerMultiplier(EDamageType damageType)
    {
        return weatherSettings.GetPowerMultiplier(weather, damageType);
    }

    private void OnTick(int time)
    {
        if (time == nextResetTime)
            weather = EWeather.None;
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        gameManager = ServiceLocator.Get<GameManager>();
        weatherSettings = ServiceLocator.Get<IAssetLoader>().Load<WeatherSettings>(nameof(weatherSettings));
    }

    private void OnEnable()
    {
        eventSystem.AddListener<int>(EEvent.OnTick, OnTick);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<int>(EEvent.OnTick, OnTick);
    }

    private void Start()
    {
        AfterWeatherChange?.Invoke(weather);
        if (nextResetTime == 0)
            nextResetTime = WeatherDuration;
    }
}
