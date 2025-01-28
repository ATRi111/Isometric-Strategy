using System.Text;
using UnityEngine;

public abstract class ValueModifier : ScriptableObject
{
    /// <summary>
    /// 假设某事发生在victim上，由victim考虑这件事对自己的价值
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
