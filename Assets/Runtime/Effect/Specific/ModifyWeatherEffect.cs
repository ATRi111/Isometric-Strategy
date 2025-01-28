using System.Collections.Generic;
using System.Text;

public class ModifyWeatherEffect : Effect
{
    private static readonly Dictionary<EWeather, string> WeatherNameDict;
    static ModifyWeatherEffect()
    {
        WeatherNameDict = new Dictionary<EWeather, string>()
        {
            { EWeather.Sunny,"晴天"},
            { EWeather.Rainy,"雨天"},
        };
    }

    public static string WeatherName(EWeather weather)
        => WeatherNameDict[weather];

    private BattleField battleField;
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

    public override bool Revokable => battleField.Weather == current;

    public override void Apply()
    {
        base.Apply();
        battleField.Weather = current;
    }

    public override void Revoke()
    {
        base.Revoke();
        battleField.Weather = prev;
    }

    public override AnimationProcess GenerateAnimation()
    {
        return null;
    }

    public override float ValueFor(PawnEntity pawn)
    {
        return 0;
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
        sb.Append("使天气由");
        sb.Append(WeatherName(prev));
        sb.Append("变为");
        sb.Append(WeatherName(current));
        sb.AppendLine();
    }
}
