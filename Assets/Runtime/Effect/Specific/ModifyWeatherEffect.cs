using System.Collections.Generic;
using System.Text;

public class ModifyWeatherEffect : Effect
{
    private static readonly Dictionary<EWeather, string> WeatherNameDict;
    static ModifyWeatherEffect()
    {
        WeatherNameDict = new Dictionary<EWeather, string>()
        {
            { EWeather.None,"������"},
            { EWeather.Sunny,"����"},
            { EWeather.Rainy,"����"},
            { EWeather.Snowy,"ѩ��"},
        };
    }

    public static string WeatherName(EWeather weather)
        => WeatherNameDict[weather];

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

    public override float ValueFor(PawnEntity pawn)
    {
        return BattleField.WeatherDuration * pawn.Brain.EvaluateWeatherUnitTime(current) 
            - Igm.BattleField.RemainingTime * pawn.Brain.EvaluateWeatherUnitTime(prev);
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
        sb.Append("ʹ������");
        sb.Append(WeatherName(prev));
        sb.Append("��Ϊ");
        sb.Append(WeatherName(current));
        sb.AppendLine();
    }
}
