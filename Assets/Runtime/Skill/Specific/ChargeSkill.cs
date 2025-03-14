using UnityEngine;

[CreateAssetMenu(fileName = "���", menuName = "����/����/���")]
public class ChargeSkill : WalkSkill
{
    public const string ChargeLevel = "������";

    public override void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.Mock(agent, igm, position, target, ret);
        int value_prev = agent.parameterDict[ChargeLevel];
        int value = value_prev + DistanceBetween(position, target);
        ModifyParameterEffect effect = new(agent, ChargeLevel, value_prev, value);
        ret.effects.Add(effect);
    }

    public override void ExtractKeyWords(KeyWordList keyWordList)
    {
        base.ExtractKeyWords(keyWordList);
        Parameter p = PawnEntity.ParameterTable[ChargeLevel];
        keyWordList.Push(p.name, p.description);
    }
}
