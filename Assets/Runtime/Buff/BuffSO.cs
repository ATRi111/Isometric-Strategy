using System.Text;
using UnityEngine;

/// <summary>
/// ���ӷ�ʽ
/// </summary>
public enum ESuperimposeMode
{
    Coexist,
    Refresh,
}

public enum EBuffType
{
    Uncertain,
    Buff,
    Debuff,
}

/// <summary>
/// ����һ��ʱ�䣬Ӱ���ɫ���ԣ���Ӧ��Ӱ��������Ķ���
/// </summary>
[CreateAssetMenu(fileName = "��״̬", menuName = "״̬/Ĭ��״̬")]
public class BuffSO : PawnModifierSO
{
    public int duration;
    public ESuperimposeMode superimposeMode = ESuperimposeMode.Refresh;
    public EBuffType buffType = EBuffType.Uncertain;
    public bool unremovable;

    public float primitiveValue;
    public ValueModifier modifier;

    protected override string TypeName => "��״̬�¿��õ�";

    /// <summary>
    /// ����victim��ʩ���˴�״̬����victim���Ǵ��¶�����ļ�ֵ
    /// </summary>
    public float ValueForVictim(PawnEntity victim)
    {
        if (modifier != null)
            return modifier.CalculateValue(primitiveValue, victim);
        return primitiveValue;
    }

    public virtual void Tick(int startTime, int currentTime)
    {

    }

    protected override void Describe(StringBuilder sb)
    {
        base.Describe(sb);
        switch(buffType)
        {
            case EBuffType.Uncertain:
                break;
            case EBuffType.Buff:
                sb.AppendLine("����״̬");
                break;
            case EBuffType.Debuff:
                sb.AppendLine("����״̬");
                break;
        }
        if (unremovable)
            sb.AppendLine("�޷����Ƴ�");
    }
}
