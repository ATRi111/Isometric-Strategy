using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "����������״̬", menuName = "״̬/����������״̬")]
public class ModifyPowerBuff : BuffSO
{
    public SkillPower power;

    public bool CanModifyPower(AimSkill skill)
    {
        if (skill.powers.Count == 0)
            return false;
        for (int i = 0; i < skill.powers.Count; i++)
        {
            if (skill.powers[i].power > 0)  //Ĭ������£�һ������Ϊ�����ļ��ܾ��ɸ����˺�
                return true;
        }
        return false;
    }

    public void ModifyPower(List<SkillPower> powers)
    {
        powers.Add(power);
    }

    public override void Register(PawnEntity pawn)
    {
        base.Register(pawn);
        pawn.OffenceComponent.ModifyPower += ModifyPower;
    }

    public override void Unregister(PawnEntity pawn)
    {
        base.Unregister(pawn);
        pawn.OffenceComponent.ModifyPower -= ModifyPower;
    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        sb.Append("�˺��༼�ܸ���");
        power.Describe(sb);
        sb.Remove(sb.Length - 2, 2);    //�Ƴ����з�
        sb.AppendLine("����");
    }
}
