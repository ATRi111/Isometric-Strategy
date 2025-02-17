using MyTool;
using System;
using System.Text;
using UnityEngine;

[Serializable]
public class RemoveBuffEffect : BuffEffect
{
    public override bool Appliable => !buffManager.Contains(buff);

    public override bool Revokable => buffManager.Contains(buff);

    public RemoveBuffEffect(Entity victim, Buff buff, BuffManager buffManager, int probability = MaxProbability) 
        : base(victim, buff, buffManager, probability)
    {
    }

    public override float ValueFor(PawnEntity pawn)
    {
        int delta = Mathf.Max(gameManager.Time - buff.endTime, 0); //delta为负数，表示buff持续时间减少
        if (PawnVictim != null)
            return delta / buff.so.duration * buff.so.ValueForVictim(PawnVictim) * pawn.Sensor.FactionCheck(victim);
        return 0f;
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

    public override void Describe(StringBuilder sb, bool result)
    {
        base.Describe(sb, result);
        sb.Append("获得剩余时间为");
        sb.Append(buff.endTime - gameManager.Time);
        sb.Append("的");
        sb.Append(buff.displayName);
        sb.Append("状态");
        sb.AppendLine();
    }
}
