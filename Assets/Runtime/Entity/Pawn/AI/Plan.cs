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

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine(action.skill.name);
        sb.AppendLine($"时间花费:{action.effectUnit.ActionTime}");
        return sb.ToString();
    }
}
