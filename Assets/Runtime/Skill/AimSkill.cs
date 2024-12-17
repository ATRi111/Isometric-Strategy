using MyTool;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 有目标型技能
/// </summary>
public abstract class AimSkill : Skill
{
    public const int MaxAccuracy = 100;
    public int accuracy = MaxAccuracy;
    public List<SkillPower> powers;
    public int minLayer = -2, maxLayer = 2;

    public bool LayerCheck(Vector3Int position, Vector3Int target)
    {
        int delta = target.z - position.z;
        return delta >= minLayer && delta <= maxLayer;
    }

    /// <summary>
    /// 获取技能命中的Entity
    /// </summary>
    public virtual void GetVictims(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Entity> ret)
    {
        ret.Clear();
    }

    /// <summary>
    /// 判断某个entity是否是技能允许的目标
    /// </summary>
    public virtual bool FilterVictim(Entity entity)
    {
        return true;
    }

    public override void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        base.Mock(agent, igm, position, target, ret);
        List<Entity> victims = new();
        GetVictims(agent, igm, position, target, victims);
        for (int i = 0; i < victims.Count; i++)
        {
            int r = RandomTool.GetGroup(ERandomGrounp.Battle).NextInt(1, Effect.MaxProbability + 1);
            int damage = 0;
            for (int j = 0; j < powers.Count; j++)
            {
                damage += victims[i].BattleComponent.MockDamage(powers[j].type, 100 * powers[j].power); //TODO:攻击力
            }
            HPChangeEffect effect = new(victims[i], victims[i].BattleComponent.HP, victims[i].BattleComponent.HP - damage, accuracy)
            {
                randomValue = r
            };
            ret.effects.Add(effect);
        }
    }
}
