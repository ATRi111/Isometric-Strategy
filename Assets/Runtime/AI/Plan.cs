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
            //同为影响敌人的行动，则优先选择包含更高嘲讽等级的敌人行动
            if (hatredLevel > 0 && other.hatredLevel > 0 && hatredLevel != other.hatredLevel)
                return other.hatredLevel.CompareTo(hatredLevel);
            //选择价值更高的行动
            return other.value.CompareTo(value);
        }

        //价值均为非正数时，选择时间更短的行动
        return action.Time.CompareTo(other.action.Time);
    }
}
