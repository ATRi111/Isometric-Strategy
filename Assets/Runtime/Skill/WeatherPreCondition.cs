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
        sb.Append("����");
        sb.Append(requireExist ? "��" : "����");
        sb.Append(WeatherData.WeatherName(weather));
        sb.AppendLine();
    }
}
