using Services.Event;
using System;

/// <summary>
/// µþ¼Ó·½Ê½
/// </summary>
public enum ESuperimposeMode
{
    Coexist,
    Refresh,
}

[Serializable]
public class RuntimeBuff
{
    public string buffName;
    public int startTime;
    public int endTime;

    public PawnEntity victim;
    public Buff buff;

    private bool enabled;

    public bool Enabled
    {
        get => enabled;
        set
        {
            if(enabled != value)
            {
                enabled = value;
                if(value)
                    OnEnable();
                else
                    OnDisable();
            }
        }
    }

    public RuntimeBuff(PawnEntity victim, Buff buff, int startTime)
    {
        buffName = buff.buffName;
        this.victim = victim;
        this.buff = buff;
        this.startTime = startTime;
        endTime = startTime + buff.duration;
    }

    public void Tick(int time)
    {
        buff.Tick(startTime, time);
    }
    protected void OnEnable()
    {
        buff.Register(victim);
    }
    protected void OnDisable()
    {
        buff.Unregister(victim);
    }
}
