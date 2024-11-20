using EditorExtend.GridEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Éä»÷", menuName = "¼¼ÄÜ/Éä»÷")]
public class ShootSkill : StraitLineSkill
{
    public int damage;

    public override void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.Mock(agent, igm, position, target, ret);
        GridObject gridObject = HitCheck(agent, igm, position, target);
        if(gridObject != null)
        {
            Entity victim = gridObject.GetComponentInParent<Entity>();
            if (victim != null)
            {
                int hp = victim.BattleComponent.MockDamage(agent, EDamageType.None, damage);
                Effect_HPChange effect = new(victim, victim.BattleComponent.HP, hp);
                ret.effects.Add(effect);
            }
        }
    }
}
