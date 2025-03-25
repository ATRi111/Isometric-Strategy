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
            sb.Append("ʹ������");
            sb.Append(WeatherData.WeatherName(prev));
            sb.Append("��Ϊ");
            sb.Append("����");
            sb.Append(BattleField.WeatherDuration);
            sb.Append("��");
            sb.Append(WeatherData.WeatherName(current));
            sb.AppendLine();
        }
        else
        {
            sb.Append("ʹ");
            sb.Append(WeatherData.WeatherName(current));
            sb.Append("��ʣ��ʱ��");
            sb.Append("��");
            sb.Append(battleField.WeatherRemainingTime);
            sb.Append("�ӳ�Ϊ");
            sb.Append(BattleField.WeatherDuration);
            sb.AppendLine();
        }
    }
}
