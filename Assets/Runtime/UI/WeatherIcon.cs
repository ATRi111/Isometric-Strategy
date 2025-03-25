using Services.Event;
using System.Text;

public class WeatherIcon : InfoIcon
{
    private void AfterBattleFieldChange(BattleField battleField)
    {
        WeatherData data = battleField.WeatherData;
        StringBuilder sb = new();
        sb.AppendLine(data.Name);
        if(battleField.Weather != EWeather.None)
        {
            sb.Append("持续时间:");
            sb.Append(battleField.WeatherRemainingTime);
            sb.Append(data.Description);
        }
        info = sb.ToString();
        image.sprite = data.icon;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        eventSystem.AddListener<BattleField>(EEvent.AfterBattleFieldChange, AfterBattleFieldChange);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        eventSystem.RemoveListener<BattleField>(EEvent.AfterBattleFieldChange, AfterBattleFieldChange);
    }
}
