using UnityEngine;

[CreateAssetMenu(fileName = "���", menuName = "����/���")]
public class ChargeSkill : WalkSkill
{
    public const string ParameterName = "������";

    public override void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.Mock(agent, igm, position, target, ret);
        int value_prev = agent.parameterDict[ParameterName];
        int value = value_prev + DistanceBetween(position, target);
        ModifyParameterEffect effect = new(agent, ParameterName, value_prev, value);
        ret.effects.Add(effect);
    }
}
