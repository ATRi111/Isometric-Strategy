using UnityEngine;

[CreateAssetMenu(fileName = "Éä»÷", menuName = "Skill/Éä»÷")]
public class Skill_Shoot : StraitLineSkill
{
    public int damage;

    public override void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.Mock(agent, igm, position, target, ret);
        //TODO:ÃüÖÐÅÐ¶Ï
        Entity victim = igm.EntityDict[target];
        int hp = victim.BattleComponent.MockDamage(agent, EDamageType.None, damage);
        Effect_HPChange effect = new(victim, victim.BattleComponent.HP, hp);
        ret.effects.Add(effect);
    }
}
