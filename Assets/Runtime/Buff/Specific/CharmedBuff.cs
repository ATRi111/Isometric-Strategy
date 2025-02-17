using UnityEngine;

[CreateAssetMenu(fileName = "÷È»ó", menuName = "×´Ì¬/÷È»ó")]
public class CharmedBuff : BuffSO
{
    public override void Register(PawnEntity pawn)
    {
        base.Register(pawn);
        pawn.Sensor.percievedFaction = pawn.faction switch
        {
            EFaction.Enemy => EFaction.Ally,
            EFaction.Ally => EFaction.Enemy,
            _ => EFaction.Neutral,
        };
    }

    public override void Unregister(PawnEntity pawn)
    {
        base.Unregister(pawn);
        pawn.Sensor.percievedFaction = pawn.faction;
    }
}
