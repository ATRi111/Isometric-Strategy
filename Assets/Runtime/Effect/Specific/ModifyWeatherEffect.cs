using System.Text;

public class ModifyWeatherEffect : Effect
{
    private readonly BattleField battleField;
    public EWeather prev;
    public EWeather current;

    public ModifyWeatherEffect(BattleField battleField, EWeather prev, EWeather current, int probability = 100):
        base(null, probability)
    {
        this.battleField = battleField;
        this.prev = prev;
        this.current = current;
    }

    public override bool Appliable => battleField.Weather == prev;

    public override void Apply()
    {
        base.Apply();
        battleField.Weather = current;
    }

    public override float ValueFor(PawnEntity pawn)
    {
        return BattleField.WeatherDuration * pawn.Brain.EvaluateWeatherUnitTime(current) 
            - Igm.BattleField.WeatherRemainingTime * pawn.Brain.EvaluateWeatherUnitTime(prev);
    }

    public override void Describe(StringBuilder sb, bool result)
    {
        if (hidden)
            return;
        if (!result && probability != MaxProbability)
        {
            sb.Append(probability);
            sb.Append("%");
        }

        if(prev != current)
        {
            sb.Append("使天气由");
            sb.Append(WeatherData.WeatherName(prev));
            sb.Append("变为");
            sb.Append("持续");
            sb.Append(BattleField.WeatherDuration);
            sb.Append("的");
            sb.Append(WeatherData.WeatherName(current));
            sb.AppendLine();
        }
        else
        {
            sb.Append("使");
            sb.Append(WeatherData.WeatherName(current));
            sb.Append("的剩余时间");
            sb.Append("由");
            sb.Append(battleField.WeatherRemainingTime);
            sb.Append("延长为");
            sb.Append(BattleField.WeatherDuration);
            sb.AppendLine();
        }
    }
}
