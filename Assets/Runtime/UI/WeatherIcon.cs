using Services.Event;

public class WeatherIcon : InfoIcon
{
    private void AfterBattleFieldChange(BattleField battleField)
    {
        WeatherData data = battleField.WeatherData;
        info = data.Name + "\n" + data.Description;
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
