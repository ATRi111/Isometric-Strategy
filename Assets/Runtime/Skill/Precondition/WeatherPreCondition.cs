using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "����", menuName = "����ǰ������/����")]
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
        sb.Append("��ǰ����");
        sb.Append(requireExist ? "��" : "����");
        sb.Append(WeatherData.WeatherName(weather));
        sb.AppendLine();
    }
}
