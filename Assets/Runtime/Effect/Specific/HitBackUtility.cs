using EditorExtend.GridEditor;
using MyTool;
using System.Collections.Generic;
using UnityEngine;

public static class HitBackUtility
{
    public static int CollideAttackPower = 3000;
    public static int DropAttackPower = 1000;
    public static float DefaultSpeedMultiplier = 2f;
    public static float hitBackDistance = 0.3f;

    public static void MockHitBack(IsometricGridManager igm, Vector3Int agentPosition, PawnEntity victim, int HP, int probability, EffectUnit ret)
    {
        Vector3Int victimPosition = victim.GridObject.CellPosition;
        Vector2 delta = (Vector3)(victimPosition - agentPosition);
        Vector2Int direction = EDirectionTool.NearestDirection4(delta);
        Vector2Int temp = (Vector2Int)victimPosition + direction;
        Vector3Int hitBackPosition = igm.AboveGroundPosition(temp);
        GridObject gridObject = igm.GetObjectXY(temp);
        DefenceComponent def = victim.DefenceComponent;

        int damage;
        int newHP;
        List<Vector3> route;
        if (hitBackPosition.z > victimPosition.z || gridObject == null || gridObject.TryGetComponent(out Entity _))
        {
            route = new()
            {
                victimPosition,
                Vector3.Lerp(victimPosition, hitBackPosition, hitBackDistance),
                victimPosition
            };
            hitBackPosition = victimPosition;
            damage = def.MockDamage(EDamageType.Crush, CollideAttackPower);
            newHP = Mathf.Clamp(HP - damage, 0, def.maxHP.IntValue);
        }
        else
        {
            route = new()
            {
                victimPosition,
                hitBackPosition
            };
            if (hitBackPosition.z != victimPosition.z)
                route.Insert(1, hitBackPosition.ResetZ(victimPosition.z));
            int h = Mathf.Clamp(victimPosition.z - victim.MovableGridObject.dropAbility.IntValue - hitBackPosition.z, 0, 5);
            damage = def.MockDamage(EDamageType.Crush, h * h * DropAttackPower);
            newHP = Mathf.Clamp(HP - damage, 0, def.maxHP.IntValue);
        }

        int r = Effect.NextInt();
        if (victimPosition != hitBackPosition)
        {
            MoveEffect moveEffect = new(victim, victimPosition, hitBackPosition, route, DefaultSpeedMultiplier, probability)
            {
                probability = r
            };
            ret.effects.Add(moveEffect);
        }
        if (HP != newHP)
        {
            HPChangeEffect hpChangeEffect = new(victim, HP, newHP, probability)
            {
                randomValue = r
            };
            ret.effects.Add(hpChangeEffect);
        }
    }
}
