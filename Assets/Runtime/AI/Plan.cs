using MyTool;
using System;

[Serializable]
public class Plan : IComparable<Plan>
{
    public static float ValueMinMultiplier = 0.8f;
    public static float ValueMaxMultiplier = 1.2f;
    public static RandomGroup random;

    static Plan()
    {
        random = RandomTool.GetGroup(ERandomGrounp.Battle);
    }

    public PawnAction action;
    public float primitiveValue;
    public float value;
    //对于影响敌人的行动，其嘲讽等级等于影响敌人中的最高嘲讽等级；优先选择嘲讽等级更高的行动
    public int hatredLevel;

    public Plan(PawnAction action)
    {
        this.action = action;
        primitiveValue = action.agent.Brain.Evaluate(action);
        value = primitiveValue * random.RandomFloat(ValueMinMultiplier, ValueMaxMultiplier);
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
