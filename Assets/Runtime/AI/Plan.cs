using System;

[Serializable]
public class Plan : IComparable<Plan>
{
    public PawnAction action;
    public float value;
    public int hatredLevel;

    public Plan(PawnAction action)
    {
        this.action = action;
        value = action.agent.Brain.Evaluate(action);
        hatredLevel = action.HatredLevel();
    }

    public int CompareTo(Plan other)
    {
        if (value > 0 || other.value > 0)
        {
            //ͬΪӰ����˵��ж���������ѡ��������߳���ȼ��ĵ����ж�
            if (hatredLevel > 0 && other.hatredLevel > 0 && hatredLevel != other.hatredLevel)
                return other.hatredLevel.CompareTo(hatredLevel);
            //ѡ���ֵ���ߵ��ж�
            return other.value.CompareTo(value);
        }

        //��ֵ��Ϊ������ʱ��ѡ��ʱ����̵��ж�
        return action.Time.CompareTo(other.action.Time);
    }
}
