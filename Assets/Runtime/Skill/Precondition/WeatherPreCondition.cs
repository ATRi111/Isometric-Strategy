using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "天气", menuName = "技能前置条件/天气")]
public class WeatherPreCondition : SkillPrecondition
{
    public EWeather weather;
    public bool requireExist;

    public override bool Verify(IsometricGridManager igm, PawnEntity agent)
    {
        if (requireExist)
            return igm.BattleField.Weather == weather;
        return igm.BattleField.Weather != weather;
    }

    public override void Describe(StringBuilder sb)
    {
        sb.Append("当前天气");
        sb.Append(requireExist ? "是" : "不是");
        sb.Append(WeatherData.WeatherName(weather));
        sb.AppendLine();
    }
}
