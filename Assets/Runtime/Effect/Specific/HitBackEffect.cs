using EditorExtend.GridEditor;
using MyTool;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class HitBackEffect : MoveEffect
{
    public static int CollideAttackPower = 3000;
    public static int DropAttackPower = 1000;
    public static float DefaultSpeedMultiplier = 2f;
    public static float hitBackDistance = 0.3f;

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

    public override AnimationProcess GenerateAnimation()
    {
        FollowHPBar HPBar = victim.GetComponentInChildren<FollowHPBar>();
        Vector3 position = HPBar.UseDamageNumberPosition();
        return new HitBackAnimationProcess(this, HPBar.transform, position, prevHP - currentHP);
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

    public override void Describe(StringBuilder sb, bool result)
    {
        if (hidden)
            return;
        if (!result && !AlwaysHappen)
        {
            sb.Append(probability);
            sb.Append("%");
        }
        sb.Append("½«");
        sb.Append(victim.gameObject.name.Bold());
        sb.Append("»÷ÍË"); 
        if (currentHP < prevHP)
        {
            sb.Append("£¬");
            sb.Append("ÔÙÔì³É");
            sb.Append(prevHP - currentHP);
            if (to != from)
                sb.Append("µã×¹ÂäÉËº¦");
            else
                sb.Append("µãÅö×²ÉËº¦");
        }
        sb.AppendLine();
    }
}
