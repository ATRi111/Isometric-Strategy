using System;
using System.Text;

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

    public override bool Appliable => victim.DefenceComponent.HP == prev;

    protected override AnimationProcess GenerateAnimation_Local()
    {
        return new ObjectAnimationProcess(this,
            "�����Ч_Ѫ���ı�",
            victim.transform,
            victim.transform.position);
    }

    public override void Apply()
    {
        base.Apply();
        victim.DefenceComponent.HP = current;
    }

    public override float ValueFor(PawnEntity pawn)
    {
        return (current - prev) * pawn.Sensor.FactionCheck(victim);
    }

    public override void Describe(StringBuilder sb, bool result)
    {
        base.Describe(sb, result);
        sb.Append("��������");
        sb.Append(prev);
        sb.Append("��Ϊ");
        sb.Append(current);
        sb.AppendLine();
    }
}
