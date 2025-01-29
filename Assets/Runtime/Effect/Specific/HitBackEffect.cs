using EditorExtend.GridEditor;
using MyTool;
using System.Collections.Generic;
using UnityEngine;

public class HitBackEffect : MoveEffect
{
    public static int CollideAttackPower = 3000;
    public static int DropAttackPower = 2000;
    public static float DefaultSpeedMultiplier = 2f;

    public int prevHP;
    public int currentHP;
    public override bool Appliable => base.Appliable && victim.DefenceComponent.HP == prevHP;
    public override bool Revokable => base.Revokable && victim.DefenceComponent.HP == currentHP;

    public static HitBackEffect MockHitBack(IsometricGridManager igm, Vector3Int agentPosition, PawnEntity victim, int HP, int probability)
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
        List<Vector3> route = null;
        if (hitBackPosition.z > victimPosition.z || gridObject.TryGetComponent(out Entity _))
        {
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
            int h = Mathf.Max(victim.MovableGridObject.dropAbility.IntValue - victimPosition.z + hitBackPosition.z, 0);
            damage = def.MockDamage(EDamageType.Crush, h * DropAttackPower);
            newHP = Mathf.Clamp(HP - damage, 0, def.maxHP.IntValue);
        }

        return new HitBackEffect(victim, victimPosition, hitBackPosition, route, HP, newHP, DefaultSpeedMultiplier, probability);
    }

    public HitBackEffect(PawnEntity victim, Vector3Int from, Vector3Int to, List<Vector3> route, int prevHP, int currentHP, float speedMultiplier = 1, int probability = 100)
        : base(victim, from, to, route, speedMultiplier, probability)
    {
        this.prevHP = prevHP;
        this.currentHP = currentHP;
    }


    public override void Apply()
    {
        base.Apply();
        victim.DefenceComponent.HP = currentHP;
    }

    public override void Revoke()
    {
        base.Revoke();
        victim.DefenceComponent.HP = prevHP;
    }

    public override float ValueFor(PawnEntity pawn)
    {
        return base.ValueFor(pawn) + (currentHP - prevHP) * pawn.FactionCheck(victim);
    }
}
