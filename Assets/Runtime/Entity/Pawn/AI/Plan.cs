using System;
using System.Text;

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
        return other.value.CompareTo(value);
    }
}
