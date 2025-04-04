using Services.Event;
using System.Text;

public class WeatherIcon : InfoIcon
{
    private void AfterWeatherChange(BattleField battleField)
    {
        WeatherData data = battleField.WeatherData;
        StringBuilder sb = new();
        sb.AppendLine(data.Name);
        if(battleField.Weather != EWeather.None)
        {
            sb.Append("持续时间:");
            sb.Append(battleField.WeatherRemainingTime);
            sb.AppendLine();
            sb.Append(data.Description);
        }
        info = sb.ToString();
        image.sprite = data.icon;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        eventSystem.AddListener<BattleField>(EEvent.AfterWeatherChange, AfterWeatherChange);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        eventSystem.RemoveListener<BattleField>(EEvent.AfterWeatherChange, AfterWeatherChange);
    }
}
