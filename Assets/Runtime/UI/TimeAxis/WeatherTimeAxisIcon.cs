using Services.Event;
using UnityEngine;

public class WeatherTimeAxisIcon : InfoIcon
{
    private TimeAxisUI timeAxisUI;

    public void OnTick(int time)
        => UpdatePosition();

    public void AfterWeatherChange(BattleField _)
        => UpdatePosition();

    public void UpdatePosition()
    {
        BattleField battleField = IsometricGridManager.Instance.BattleField;
        if (battleField.WeatherRemainingTime == 0)
        {
            info = string.Empty;
            transform.position = new Vector3(-1000, 0, 0);
        }
        else
        {
            info = $"当前天气结束\n剩余时间:{battleField.WeatherRemainingTime}";
            transform.position = timeAxisUI.PercentToPosition(battleField.WeatherRemainingTime);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        timeAxisUI = GetComponentInParent<TimeAxisUI>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        eventSystem.AddListener<int>(EEvent.OnTick, OnTick);
        eventSystem.AddListener<BattleField>(EEvent.AfterWeatherChange, AfterWeatherChange);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        eventSystem.RemoveListener<int>(EEvent.OnTick, OnTick);
        eventSystem.RemoveListener<BattleField>(EEvent.AfterWeatherChange, AfterWeatherChange);
    }
}
