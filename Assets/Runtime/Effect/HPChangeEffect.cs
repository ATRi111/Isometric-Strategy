using System;

[Serializable]
public class HPChangeEffect : Effect
{
    public int prev, current;

    public HPChangeEffect(Entity victim, int prev, int current, int probability = MaxProbability)
        : base(victim, probability)
    {
        this.prev = prev;
        this.current = current;
    }

    public override bool Appliable => victim.BattleComponent.HP == prev;

    public override bool Revokable => victim.BattleComponent.HP == current;

    public override AnimationProcess GenerateAnimation()
    {
        return null;    //TODO
    }

    public override void Apply()
    {
        base.Apply();
        victim.BattleComponent.HP = current;
        if (current == 0)
            victim.Die();
    }

    public override void Revoke()
    {
        base.Revoke();
        victim.BattleComponent.HP = prev;
        if(current == 0)
            victim.Revive();
    }

    public override float PrimitiveValueFor(PawnEntity pawn)
    {
        return (current - prev) * pawn.CheckFaction(victim);
    }
}
