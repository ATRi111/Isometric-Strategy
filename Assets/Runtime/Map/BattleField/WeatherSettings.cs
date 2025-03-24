using MyTool;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum EWeather
{
    None,
    Sunny,
    Rainy,
    Snowy
}

[Serializable]
public class WeatherData
{
    public static string WeatherName(EWeather weather)
    {
        return weather switch
        {
            EWeather.None => "������" ,
            EWeather.Sunny => "����" ,
            EWeather.Rainy => "����" ,
            EWeather.Snowy => "ѩ��" ,
            _ => ""
        };
    }

    public EWeather weather;
    [SerializeField]
    private SerializedDictionary<EDamageType, float> powerMultiplierDict;

    public float PowerMultiplier(EDamageType damageType)
    {
        if (powerMultiplierDict.ContainsKey(damageType))
            return powerMultiplierDict[damageType];
        return 1f;
    }

    public string Description
    {
        get
        {
            StringBuilder sb = new();
            foreach (var pair in powerMultiplierDict)
            {
                string stype = SkillPower.DamageTypeName(pair.Key);
                sb.Append(stype);
                sb.Append("�˺���Ϊ");
                sb.Append(pair.Value.ToString("P0"));
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}

public class WeatherSettings : ScriptableObject
{
    public List<WeatherData> dataList;

    public WeatherData GetWeatherData(EWeather weather)
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].weather == weather)
                return dataList[i];
        }
        return null;
    }

    public float GetPowerMultiplier(EWeather weather, EDamageType damageType)
    {
        WeatherData data = GetWeatherData(weather);
        if (data != null)
            return data.PowerMultiplier(damageType);
        return 1f;
    }

    public string GetDescription(EWeather weather)
    {
        WeatherData data = GetWeatherData(weather);
        if (data != null)
            return data.Description;
        return string.Empty;
    }
}
