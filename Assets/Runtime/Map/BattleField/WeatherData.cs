using MyTool;
using System;
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
            EWeather.None => "无天气",
            EWeather.Sunny => "晴天",
            EWeather.Rainy => "雷雨",
            EWeather.Snowy => "下雪",
            _ => ""
        };
    }

    public Sprite icon;
    public EWeather weather;
    public string Name => WeatherName(weather);

    [SerializeField]
    private SerializedDictionary<EDamageType, float> powerMultiplierDict;

    /// <summary>
    /// 此天气下某种伤害类型的倍率
    /// </summary>
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
                sb.Append("伤害变为");
                sb.Append(pair.Value.ToString("P0"));
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
