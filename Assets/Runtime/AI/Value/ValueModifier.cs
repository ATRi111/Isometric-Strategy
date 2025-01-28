using System.Text;
using UnityEngine;

public abstract class ValueModifier : ScriptableObject
{
    /// <summary>
    /// ����ĳ�·�����victim�ϣ���victim��������¶��Լ��ļ�ֵ
    /// </summary>
    public abstract float CalculateValue(float primitiveValue, PawnEntity victim);

    public string Description
    {
        get
        {
            StringBuilder sb = new();
            Describe(sb);
            return sb.ToString();
        }
    }

    protected abstract void Describe(StringBuilder sb);
}
