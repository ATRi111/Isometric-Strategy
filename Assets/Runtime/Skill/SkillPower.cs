using System.Text;
using UnityEngine;

[System.Serializable]
/// <summary>
/// ��������
/// </summary>
public struct SkillPower
{
    public static string DamageTypeName(EDamageType type)
    {
        return type switch
        {
            EDamageType.Slash => "�ж�",
            EDamageType.Pierce => "����",
            EDamageType.Crush => "���",
            EDamageType.Fire => "��",
            EDamageType.Ice => "��",
            EDamageType.Lightning => "��",
            _ => string.Empty
        };
    }

    public EDamageType type;
    public int power;
    public float strMultiplier;
    public float dexMultiplier;
    public float intMultiplier;
    public float mndMultiplier;
    
    public void Describe(StringBuilder sb)
    {
        void Split()
        {
            sb.Append(" ");
        }

        string stype = DamageTypeName(type);
        if (!string.IsNullOrEmpty(stype))
        {
            sb.Append("(");
            sb.Append(stype);
            sb.Append(")");
        }

        sb.Append(Mathf.Abs(power));
        
        sb.Append("(");
        if(strMultiplier != 0)
        {
            sb.Append(Mathf.RoundToInt(100 * strMultiplier));
            sb.Append("%");
            sb.Append("�����ӳ�");
            Split();
        }
        if (dexMultiplier != 0)
        {
            sb.Append(Mathf.RoundToInt(100 * dexMultiplier));
            sb.Append("%");
            sb.Append("���ɼӳ�");
            Split();
        }
        if (intMultiplier != 0)
        {
            sb.Append(Mathf.RoundToInt(100 * intMultiplier));
            sb.Append("%");
            sb.Append("�����ӳ�");
            Split();
        }
        if (mndMultiplier != 0)
        {
            Split();
            sb.Append(Mathf.RoundToInt(100 * mndMultiplier));
            sb.Append("%");
            sb.Append("����ӳ�");
            Split();
        }
        sb.Append(")");
        sb.AppendLine();
    }
}
