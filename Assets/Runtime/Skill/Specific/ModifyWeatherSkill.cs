using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "修改天气", menuName = "技能/特殊/修改天气")]
public class ModifyWeatherSkill : AimlessSkill
{
    public List<WeatherModifier> weatherModifiers;

    public override void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.Mock(agent, igm, position, target, ret);
        int r = Effect.NextInt();
        for (int i = 0; i < weatherModifiers.Count; i++)
        {
            if(r < weatherModifiers[i].probability)
            {
                BattleField battleField = igm.GetComponent<BattleField>();
                ModifyWeatherEffect effect = new(battleField, battleField.Weather, weatherModifiers[i].weather);
                ret.effects.Add(effect);
                break;
            }
            r -= weatherModifiers[i].probability;
        }
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        for (int i = 0;i < weatherModifiers.Count;i++)
        {
            weatherModifiers[i].Describe(sb);
        }
    }
}
