using Services;
using System;
using System.Collections.Generic;

public class GameManager : Service,IService
{
    public override Type RegisterType => GetType();

    private readonly HashSet<PawnEntity> pawns = new();

    public Action<int,int> AfterTimeChange;
    private int time;
    public int Time
    {
        get => time; 
        set
        {
            if(time != value)
            {
                int prev = time;
                time = value;
                AfterTimeChange.Invoke(prev, time);
            }
        }
    }

    public void Register(PawnEntity entity)
    {
        pawns.Add(entity);
    }
    public void Unregister(PawnEntity entity)
    {
        pawns.Remove(entity);
    }

    public bool PushOn()
    {
        Time++;
        foreach(PawnEntity pawn in pawns)
        {
            if (Time >= pawn.State.waitTime)
            {
                pawn.DoAction();
                return true;
            }
        }
        return false;
    }
}
