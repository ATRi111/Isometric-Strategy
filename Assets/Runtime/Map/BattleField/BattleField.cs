using Services;
using Services.Asset;
using Services.Event;
using Services.ObjectPools;
using System;
using UnityEngine;

public class BattleField : MonoBehaviour
{
    public static int WeatherDuration = 250;

    private IEventSystem eventSystem;
    private GameManager gameManager;
    private IObjectManager objectManager;

    [NonSerialized]
    public WeatherSettings weatherSettings;

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
                eventSystem.Invoke(EEvent.AfterWeatherChange, this);
            }
        }
    }

    public WeatherData WeatherData => weatherSettings[weather];

    /// <summary>
    /// 当前天气的剩余时间
    /// </summary>
    public int WeatherRemainingTime
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
        return weatherSettings[weather].PowerMultiplier(damageType);
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
        objectManager = ServiceLocator.Get<IObjectManager>();
        weatherSettings = ServiceLocator.Get<IAssetLoader>().Load<WeatherSettings>(nameof(WeatherSettings));
        weatherSettings.Init();
        objectManager.Activate(nameof(WeatherAnimationController), transform.position, Vector3.zero, transform);
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
        if (nextResetTime == 0)
            nextResetTime = WeatherDuration;
        eventSystem.Invoke(EEvent.AfterWeatherChange, this);
    }
}
