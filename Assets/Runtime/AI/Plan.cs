using System;

[Serializable]
public class Plan : IComparable<Plan>
{
    public PawnAction action;
    public float value;

    public Plan(PawnAction action)
    {
        this.action = action;
        value = action.agent.Brain.Evaluate(action);
    }

    public int CompareTo(Plan other)
    {
        if (value > 0 || other.value > 0)
            return other.value.CompareTo(value);

        return action.Time.CompareTo(other.action.Time);
    }
}
