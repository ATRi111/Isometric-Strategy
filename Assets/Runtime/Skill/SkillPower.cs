using System.Text;
using UnityEngine;

[System.Serializable]
/// <summary>
/// 技能威力
/// </summary>
public struct SkillPower
{
    public static string DamageTypeName(EDamageType type)
    {
        return type switch
        {
            EDamageType.Slash => "切断",
            EDamageType.Pierce => "穿刺",
            EDamageType.Crush => "打击",
            EDamageType.Fire => "火",
            EDamageType.Ice => "冰",
            EDamageType.Lightning => "雷",
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
            sb.Append("力量加成");
            Split();
        }
        if (dexMultiplier != 0)
        {
            sb.Append(Mathf.RoundToInt(100 * dexMultiplier));
            sb.Append("%");
            sb.Append("灵巧加成");
            Split();
        }
        if (intMultiplier != 0)
        {
            sb.Append(Mathf.RoundToInt(100 * intMultiplier));
            sb.Append("%");
            sb.Append("智力加成");
            Split();
        }
        if (mndMultiplier != 0)
        {
            Split();
            sb.Append(Mathf.RoundToInt(100 * mndMultiplier));
            sb.Append("%");
            sb.Append("精神加成");
            Split();
        }
        sb.Append(")");
        sb.AppendLine();
    }
}
