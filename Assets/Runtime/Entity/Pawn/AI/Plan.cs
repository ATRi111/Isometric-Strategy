using System;

[Serializable]
public class Plan : IComparable<Plan>
{
    public PawnAction action;
    public float value;

    public Plan(PawnAction action)
    {
        this.action = action;
        value = action.agent.Brain.Evaluate(action.effectUnit);
    }

    public int CompareTo(Plan other)
    {
        if (value <= 0 && other.value <= 0)
        {
            int flag = 0;
            if (action.skill is MoveSkill)
                flag--;
            if (other.action.skill is MoveSkill)
                flag++;
            if (flag != 0)
                return flag;
        }
        return other.value.CompareTo(value);
    }
}
