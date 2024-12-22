using System;

[Serializable]
public class AddBuffEffect : BuffEffect
{
    public override bool Appliable => !buffManager.Contains(buff);

    public override bool Revokable => buffManager.Contains(buff);

    public AddBuffEffect(Entity victim, Buff buff, BuffManager buffManager, int probability = MaxProbability) 
        : base(victim, buff, buffManager, probability)
    {
    }

    public override float PrimitiveValueFor(PawnEntity pawn)
    {
        return buff.so.primitiveValue * pawn.FactionCheck(victim);
    }

    public override void Apply()
    {
        base.Apply();
        buffManager.Add(buff);
    }

    public override void Revoke()
    {
        base.Revoke();
        buffManager.Remove(buff);
    }
}
