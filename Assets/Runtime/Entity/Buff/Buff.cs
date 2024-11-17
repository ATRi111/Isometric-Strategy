using UnityEngine;

public class Buff : ScriptableObject
{
    public string buffName;
    public int duration;

    public virtual void Tick(int startTime, int currentTime)
    {

    }

    public virtual void Register(Entity victim)
    {

    }

    public virtual void Unregister(Entity victim)
    {

    }
}
