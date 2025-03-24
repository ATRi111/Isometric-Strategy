using System;
using System.Collections.Generic;
using UnityEngine;

public enum EWeather
{
    None,
    Sunny,
    Rainy,
    Snowy
}

[Serializable]
public struct WeatehrDamageData
{
    public float damageMultiplier;
    public EDamageType damageType;
    public EWeather weather;

    public override readonly bool Equals(object obj)
    {
        if (obj is WeatehrDamageData data)
        {
            return damageType == data.damageType && weather == data.weather;
        }
        return false;
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(weather, damageType);
    }
}

public class WeatherSettings : ScriptableObject
{
    public List<WeatehrDamageData> dataList;

    public float PowerMultiplier(EWeather weather, EDamageType damageType)
    {
        WeatehrDamageData data = new()
        {
            damageType = damageType,
            weather = weather,
        };
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].Equals(data))
                return dataList[i].damageMultiplier;
        }
        return 1f;
    }
}
