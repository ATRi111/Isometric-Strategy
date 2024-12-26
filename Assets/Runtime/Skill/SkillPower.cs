using System.Text;
using UnityEngine;

[System.Serializable]
/// <summary>
/// 技能威力
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
            EDamageType.Slash => "切断",
            EDamageType.Pierce => "穿刺",
            EDamageType.Crush => "打击",
            EDamageType.Fire => "火",
            EDamageType.Ice => "冰",
            EDamageType.Lightning => "雷",
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
            sb.Append("力量加成");
            flag = true;
        }
        if (dexMultiplier != 0)
        {
            Split();
            sb.Append(Mathf.RoundToInt(100 * dexMultiplier));
            sb.Append("%");
            sb.Append("灵巧加成");
            flag = true;
        }
        if (intMultiplier != 0)
        {
            Split();
            sb.Append(Mathf.RoundToInt(100 * intMultiplier));
            sb.Append("%");
            sb.Append("智力加成");
            flag = true;
        }
        if (mndMultiplier != 0)
        {
            Split();
            sb.Append(Mathf.RoundToInt(100 * mndMultiplier));
            sb.Append("%");
            sb.Append("精神加成");
        }
        sb.Append(")");
        sb.AppendLine();
    }
}
