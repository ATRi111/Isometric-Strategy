using Character;
using MyTool;
using Services;
using UnityEngine;

public class BuffManager : CharacterComponentBase
{
    private PawnEntity pawn;
    private GameManager gameManager;
    [SerializeField]
    private SerializedHashSet<Buff> buffs;

    public void OnTick(int time)
    {
        foreach (Buff buff in buffs)
        {
            buff.Enabled = time < buff.endTime;
            if (buff.Enabled)
                buff.Tick(time);
        }
    }

    public BuffEffect MockAdd(BuffSO so, PawnEntity victim, int probability)
    {
        Buff buff = new(victim, so, gameManager.Time);
        switch(so.superimposeMode)
        {
            case ESuperimposeMode.Coexist:
                return new AddBuffEffect(pawn, buff, this, probability);
            case ESuperimposeMode.Refresh:
                Buff existed = SuprimposeCheck(buff);
                if(existed == null)
                    return new AddBuffEffect(pawn, buff, this, probability);
                return new LengthenBuffEffect(pawn, existed, existed.endTime, this, probability);
            default:
                throw new System.ArgumentException();
        }
    }

    public bool Contains(Buff target)
    {
        return buffs.Contains(target);
    }

    public Buff FindEnabled(BuffSO so)
    {
        foreach(Buff buff in buffs)
        {
            if(buff.so == so && buff.Enabled)
                return buff;
        }
        return null;
    }

    public Buff SuprimposeCheck(Buff target)
    {
        foreach(Buff buff in buffs)
        {
            if(target.SuperimposeCheck(buff))
                return buff;
        }
        return null;
    }

    public void Add(Buff buff)
    {
        buffs.Add(buff);
        buff.Enabled = gameManager.Time < buff.endTime;
    }

    public void Remove(Buff buff)
    {
        buff.Enabled = false;
        buffs.Remove(buff);
    }

    protected override void Awake()
    {
        base.Awake();
        gameManager = ServiceLocator.Get<GameManager>();
        pawn = entity as PawnEntity;
    }
}
