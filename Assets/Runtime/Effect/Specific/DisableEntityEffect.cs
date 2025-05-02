using MyTool;
using System.Text;
using UnityEngine;

public class DisableEntityEffect : Effect
{
    public override bool Appliable => victim.gameObject.activeInHierarchy;

    public DisableEntityEffect(Entity victim, int probability = MaxProbability) : base(victim, probability)
    {
    }

    public override void Apply()
    {
        base.Apply();
        SpriteRenderer spriteRenderer = victim.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.color = spriteRenderer.color.SetAlpha(1f);
        victim.Die();
    }

    public override float ValueFor(PawnEntity pawn)
    {
        return -100f * pawn.Sensor.FactionCheck(victim);
    }

    protected override AnimationProcess GenerateAnimation_Local()
    {
        return new ObjectAnimationProcess(this,
            "�����Ч_����",
            victim.transform,
            victim.transform.position);
    }

    public override void Describe(StringBuilder sb, bool result)
    {
        base.Describe(sb, result);
        if (victim is PawnEntity)
            sb.Append("������");
        else
            sb.Append("���ƻ�");
        sb.AppendLine();
    }
}
