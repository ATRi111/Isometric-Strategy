using System;

[Serializable]
public class Buff
{
    public string buffName;
    public int startTime;
    public int endTime;

    public PawnEntity victim;
    public BuffSO so;

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
                    so.Register(victim);
                else
                    so.Unregister(victim);
            }
        }
    }

    public Buff(PawnEntity victim, BuffSO buff, int startTime)
    {
        buffName = buff.name;
        this.victim = victim;
        this.so = buff;
        this.startTime = startTime;
        endTime = startTime + buff.duration;
    }

    public void Tick(int time)
    {
        so.Tick(startTime, time);
    }

    public bool SuperimposeCheck(Buff other)
    {
        if (buffName != other.buffName)
            return false;
        if (endTime < other.startTime || other.endTime < startTime)
            return false;
        return true;
     }

    public override bool Equals(object obj)
    {
        Buff other = obj as Buff;
        if(other == null)
            return false;
        return buffName == other.buffName && startTime == other.startTime;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(buffName, startTime);
    }
}
