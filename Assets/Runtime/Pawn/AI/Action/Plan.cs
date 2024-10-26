using System;

public class Plan : IComparable<Plan>
{
    public ActionUnit actionUnit;
    public float value;

    public Plan(ActionUnit actionUnit)
    {
        this.actionUnit = actionUnit;
        value = actionUnit.agent.Brain.Evaluate(actionUnit.effectUnit);
    }

    public void Excute()
    {
        actionUnit.effectUnit.Apply();
    }

    public int CompareTo(Plan other)
    {
        return other.value.CompareTo(value);
    }
}
