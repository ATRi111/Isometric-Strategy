using Services.Event;
using System.Text;

public class WeatherIcon : InfoIcon
{
    private BattleField battleField;

    private void AfterBattleFieldChange(BattleField battleField)
    {
        this.battleField = battleField;
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

    protected override void ExtractKeyWords()
    {
        base.ExtractKeyWords();
        WeatherData data = battleField.WeatherData;
        KeyWordList.Push(data.Name, data.Description);
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
