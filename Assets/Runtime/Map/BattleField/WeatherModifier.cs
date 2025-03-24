using System.Text;

[System.Serializable]
public class WeatherModifier
{
    public EWeather weather;
    public int probability;

    public void Describe(StringBuilder sb)
    {
        sb.Append(probability);
        sb.Append("%");
        sb.Append("��������Ϊ");
        sb.Append(ModifyWeatherEffect.WeatherName(weather));
        sb.AppendLine();
    }
}