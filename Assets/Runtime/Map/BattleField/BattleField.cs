using System;
using UnityEngine;

public class BattleField : MonoBehaviour
{
    public Action<EWeather> AfterWeatherChange;

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
                AfterWeatherChange?.Invoke(weather);
            }
        }
    }

    private void Start()
    {
        AfterWeatherChange?.Invoke(weather);
    }
}
