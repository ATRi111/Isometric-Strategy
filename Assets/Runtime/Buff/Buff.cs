using MyTool;
using Services;
using System;
using System.Text;

[Serializable]
public class Buff
{
    private readonly static GameManager GameManager;
    static Buff()
    {
        GameManager = ServiceLocator.Get<GameManager>();
    }

    public string displayName;
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

    public Buff(PawnEntity victim, BuffSO so, int startTime)
    {
        displayName = so.name;
        this.victim = victim;
        this.so = so;
        this.startTime = startTime;
        endTime = startTime + so.duration;
    }

    public void Tick(int time)
    {
        so.Tick(startTime, time);
    }

    /// <summary>
    /// 检查两个Buff是否应当叠加
    /// </summary>
    public bool SuperimposeCheck(Buff other)
    {
        if (displayName != other.displayName)
            return false;
        if (endTime < other.startTime || other.endTime < startTime)
            return false;
        return true;
    }

    //同名且起始时间相同的buff只能存在一个
    public override bool Equals(object obj)
    {
        if (obj is not Buff other)
            return false;
        return displayName == other.displayName && startTime == other.startTime;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(displayName, startTime);
    }

    public string Description
    {
        get
        {
            StringBuilder sb = new();
            Describe(sb);
            return sb.ToString();
        }
    }

    private void Describe(StringBuilder sb)
    {
        sb.AppendLine(displayName.Bold());
        sb.Append("剩余时间:");
        sb.AppendLine((endTime - GameManager.Time).ToString());
        sb.AppendLine();
        sb.Append(so.Description);
    }
}
