using System.Text;
using UnityEngine;

[System.Serializable]
/// <summary>
/// ��������
/// </summary>
public class SkillPower
{
    public EDamageType type;
    public int power;
    public float strMultiplier;
    public float dexMultiplier;
    public float intMultiplier;
    public float mndMultiplier;
    
    public void Describe(StringBuilder sb)
    {
        bool flag = false;
        void Split()
        {
            if (flag)
                sb.Append(" ");
            flag = false;
        }

        string stype = type switch
        {
            EDamageType.Slash => "�ж�",
            EDamageType.Pierce => "����",
            EDamageType.Crush => "���",
            EDamageType.Fire => "��",
            EDamageType.Ice => "��",
            EDamageType.Lightning => "��",
            _ => string.Empty
        };
        if(!string.IsNullOrEmpty(stype))
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
            flag = true;
        }
        if (dexMultiplier != 0)
        {
            Split();
            sb.Append(Mathf.RoundToInt(100 * dexMultiplier));
            sb.Append("%");
            sb.Append("���ɼӳ�");
            flag = true;
        }
        if (intMultiplier != 0)
        {
            Split();
            sb.Append(Mathf.RoundToInt(100 * intMultiplier));
            sb.Append("%");
            sb.Append("�����ӳ�");
            flag = true;
        }
        if (mndMultiplier != 0)
        {
            Split();
            sb.Append(Mathf.RoundToInt(100 * mndMultiplier));
            sb.Append("%");
            sb.Append("����ӳ�");
        }
        sb.Append(")");
        sb.AppendLine();
    }
}
