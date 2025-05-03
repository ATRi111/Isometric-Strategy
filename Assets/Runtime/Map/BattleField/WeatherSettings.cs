using System;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSettings : ScriptableObject
{
    [SerializeField]
    private List<WeatherData> dataList;

    public WeatherData this[EWeather weather]
    {
        get
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].weather == weather)
                    return dataList[i];
            }
            throw new Exception();
        }
    }

    public void Init()
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            dataList[i].Init();
        }
    }
}
