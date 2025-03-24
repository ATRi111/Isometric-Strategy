using System.Text;

[System.Serializable]
public class WeatherPreCondition
{
    public EWeather weather;
    public bool requireExist;

    public bool Verify(BattleField battleField)
    {
        if(requireExist)
            return battleField.Weather == weather;
        return battleField.Weather != weather;
    }

    public void Describe(StringBuilder sb)
    {
        sb.Append("天气");
        sb.Append(requireExist ? "是" : "不是");
        sb.Append(ModifyWeatherEffect.WeatherName(weather));
        sb.AppendLine();
    }
}
